using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ThridWolrdShooterGame.Managers;

namespace ThridWolrdShooterGame.Entites.Messages
{
    public class StaticMessage:Message
    {
        public StaticMessage(Vector2 Position, string Message)
        {
            this.Font = EntityManager.Resources.DemoFont;
            this.MessageString = Message;
        }
    }
}
