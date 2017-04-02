using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ThridWolrdShooterGame.Entites;
using ThridWolrdShooterGame.Entites.Enemies;
using ThridWolrdShooterGame.Entites.Items;
using ThridWolrdShooterGame.Entites.Messages;

namespace ThridWolrdShooterGame
{
    public class QuadTree
    {
        private int MAX_OBJECTS = 50; // 50
        private int MAX_LEVELS = 5; //10

        private int level;
        private List<Entity> objects;
        private Rectangle bounds;
        private QuadTree[] nodes;

        int subWidth;
        int subHeight;
        int x;
        int y;
        /// <summary>
        /// Creates a new instance of the Quadtree class.
        /// </summary>
        /// <param name="pLevel">Current node level</param>
        /// <param name="pBounds">Represents the 2D space that the node occupies</param>
        public QuadTree(int pLevel, Rectangle pBounds)
        {
            level = pLevel;
            objects = new List<Entity>();
            bounds = pBounds;
            nodes = new QuadTree[4];

            //Console.WriteLine ("New Nodes has been added.");
        }


        /// <summary>
        /// Clears the Quadtree
        /// </summary>
        public void clear()
        {
            objects.Clear();

            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i] != null)
                {
                    nodes[i].clear();
                    nodes[i] = null;
                }
            }
        }

        /// <summary>
        /// Splits the node into 4 subnodes
        /// </summary>
        private void split()
        {
            /*
             * So this is the waythe cuadrants are divided
             * ---------
             * | 1 | 0 |
             * ---------
             * | 2 | 3 |
             * ---------
             * 
             */
            subWidth = (int)(bounds.Width / 2);
            subHeight = (int)(bounds.Height / 2);
            x = (int)bounds.X;
            y = (int)bounds.Y;


            /*
             * The cuadrants needs to be made of the parent rectangle. duh!.
             */
            nodes[0] = new QuadTree(level + 1, new Rectangle(x + subWidth, y, subWidth, subHeight));
            nodes[1] = new QuadTree(level + 1, new Rectangle(x, y, subWidth, subHeight));
            nodes[2] = new QuadTree(level + 1, new Rectangle(x, y + subHeight, subWidth, subHeight));
            nodes[3] = new QuadTree(level + 1, new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight));
        }

        /// <summary>
        /// Determine which node the object belongs to. -1 means
        /// object canno completely fit within a child node and is part 
        /// of the parent node
        /// </summary>
        /// <returns>The index</returns>
        /// <param name="pRect">Target Rectangle that represents a node</param>
        private int getIndex(Entity entity)
        {
            // -1 means that it cannot be fit into a quadrant. So let's leave that item.
            // into the parent quadrant.
            int index = -1;

            /*
             * Here it's time to made the bounds of each quadrant.
             */
            double verticalMidpoint = bounds.X + (bounds.Width / 2);
            double horizontalMidpoint = bounds.Y + (bounds.Height / 2);

            /*
             * First we will see if the object can fit into the top or bottom quadrants.
             */
            // Object can completely fit within the top quadrants
            bool topQuadrant = (entity.Position.Y < horizontalMidpoint && entity.Position.Y + entity.Size.Y < horizontalMidpoint);
            // Object can completely fit within the bottom quadrants
            bool bottomQuadrant = (entity.Position.Y > horizontalMidpoint);


            /*
             * Now that we know if the item it's wheter top or bottom. Next we need to know if 
             * can fit into the Most left or most right quadrants.
             */
            // Object can completely fit within the left quadrants
            if (entity.Position.X < verticalMidpoint && entity.Position.X + entity.Size.X < verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = 1;
                }
                else if (bottomQuadrant)
                {
                    index = 2;
                }
            }
            // Object can completely fit within the right quadrants
            else if (entity.Position.X > verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = 0;
                }
                else if (bottomQuadrant)
                {
                    index = 3;
                }
            }

            return index;
        }

        /// <summary>
        /// Insert the object into the quadtree. If the node
        /// exceeds the capcacity, it will split and add all
        /// objects to their corresponding nodes.
        /// </summary>
        /// <param name="pRect">P rect.</param>
        public void insert(Entity entity)
        {
            /*
             * First let's check if the node has Childs. if So add
             * the element to a child.
             */
            if (nodes[0] != null)
            { // ¿Do we have childs?
                int index = getIndex(entity);

                if (index != -1)
                { // if the element can fit into a child let's add it.
                    nodes[index].insert(entity);
                    // If the element couldn't fit into a child then we add it to the parent
                    /*
                     * Now that we added the element into the child.
                     * there's nothing else to do. So let's quit this method
                     */
                    return;
                }
            }

            /*
             * If we don't have childs then add the element into the current level.
             */
            objects.Add(entity);

            /*
             * Now that we added the method into the current level. Let's do a check
             * if we can still add elements to this level.
             * 
             * In case that we reached the limit then it's time to make Childs. XD.
             */
            if (objects.Count > MAX_OBJECTS && level < MAX_LEVELS)
            {
                if (nodes[0] == null)
                {
                    split(); // Create the childs
                }

                /*
                 * Now that we have created childs we have to move the elements
                 * of this level to the childs. So let's iterate in the list of elements
                 * from the current level and put them into the child nodes
                 */
                int i = 0;
                while (i < objects.Count)
                {
                    //Get a new index.
                    int index = getIndex(objects[i]);
                    //If the element can be fully placed in a node, well let's do it!.
                    if (index != -1)
                    {
                        //Add the element to the child.
                        nodes[index].insert(objects[i]);
                        //Remove the element from this level. We dont need it  cause the childs has it.
                        objects.RemoveAt(i);
                    }
                    else
                    {
                        // If the element couldn't fit into a child node,then let's leave into this current level.
                        // which is the PArent.
                        i++;
                    }
                }
            }
        }

        /// <summary>
        /// Return all objects that could collide with the given object
        /// </summary>
        /// <param name="returnObjects">Return objects.</param>
        /// <param name="pRect">P rect.</param>
        public List<Entity> retrieve(List<Entity> returnObjects, Entity entity)
        {
            bool targetFound = false;
            /*
             * First, let's retrieve the index of the desired item. 
             */
            int index = getIndex(entity);

            // Now that we know the index. let's check into the child nodes (in case the index show's that).
            if (index != -1 && nodes[0] != null)
            {
                //This will retrieve the objects that the main item might collide with form the child's nodes.
                nodes[index].retrieve(returnObjects, entity);
            }


            foreach (Entity target in objects)
            {
                if (target is Bullet && entity is Bullet)
                    continue;

                if (target is Enemy && entity is Item)
                    continue;

                if (target is Enemy && entity is Amigo)
                    continue;

                if (entity is Bullet)
                {
                    if ((entity as Bullet).Type == Bullet.BULLETTYPE.ENEMY && target is Enemy)
                        continue;

                    if ((entity as Bullet).Type == Bullet.BULLETTYPE.PLAYER && target is PlayerShip)
                        continue;

                    if ((entity as Bullet).Type == Bullet.BULLETTYPE.PLAYER && target is Amigo)
                        continue;

                    if (entity is Bullet && target is Item)
                        continue;
                }

                if (target is FadingOutMessage)
                    continue;

                if (target is ItemPointer)
                    continue;

                //if(target is PlayerShip)
                if (!targetFound)
                {
                    if (target.Position == entity.Position)
                    {
                        targetFound = true;
                        continue;
                    }
                }

                returnObjects.Add(target);
            }
            // Now that 
            //objects.ForEach (x => returnObjects.Add(x));
            return returnObjects;
        }
    }
}
