# Infinite pew pew - A tiny fast paced twin stick shooter


<img src="https://www.dropbox.com/s/w5sb3pyeef64ns6/Feature.jpg?dl=1" align="center">


Infinite pew pew is a clone of Geometry Wars with new mechanics. Experience an intense gameplay, full of danger in every corner you go (or escape), that will boil your blood and augment your adrenaline with hordes of enemies while dodging bullets and surviving as long as you can on this geometric mayhem of massacre and destruction.

### Features:

- 5 kinds of weapons
- Weapon upgrades
- 14 types of enemies
- Recruit Amigos (yeah, that's an actual item name in the game)
- Badass Time (a power up that slows down the game for a short period of time)
- Prepare to die a lot

### Controls: 

- W,A,S,D - Move your ship
- Directional Arrows - Shooting directions
- Enter: Action Button
- ESC: Cancel Button
- Infinite pew pew also supports the xbox360 controller

Infinite pew pew is made with RIE, my own engine built with XNA, so in order to execute Infinite pew pew you have to install XNA on your windows computer. The process is different depending on your OS, so I recommend you to google it on how to do it. 

The objective of uploading the source code of Infinite pew pew is that you, young adventurers, want to build a Game Engine or a Game from Scrath know how to do it. Hopefully this code will help you solve some bugs, or give you an idea on how to build certain module in your application. If you have any questons feel free to send me a mail to alan.sosa.mejia@gmail, I'd be glad to help you in your project. 

=)

## Contents

- [History](#history)
- [Download Infinite pew pew](#download)
- [Getting Started](#getting-started)
- [Building Infinite pew pew](#building)
- [Documentation](#documentation)
- [Requeriments](#requeriments)
- [Contributing](#contributing)
- [Credits](#credits)

<a name="history"></a>
![history](http://s24.postimg.org/5tj8t4bhx/history-header.png "history")

Infinite pew pew began as a programming adventure, an education toy and as a playground where I could practice all the things related to game engines that I learned thanks to the internet. After a couple of weeks the project became more solid and so the decision to make it commercial became closer with the support of my family and friends.

2 years have passed since this project has finished, I have made some revenue of it, and if you want to know more about the story visit the [Blog](http://cubitomorado.blogspot.com) that I was writing when this project was alive. I learned so much from this: Business development, Importance of distribution channels, how deadly is to code a game engine and above all, I had tons of fun even thou I was starving most of the time. So enjoy this little piece of code and I hope is useful to you. 

<a name="download"></a>
![Download Infinite pew pew](http://s30.postimg.org/pa6rbdfc1/download_header.png "Download Infinite pew pew")

All Infinite pew pew versions are [hosted on Github](http://github.com/AlanSosa/Infinite-pew-pew-PC-Version). You can:

* Clone the git repository via [https](https://github.com/AlanSosa/Infinite-pew-pew-PC-Version.git) or with the Github [Windows](github-windows://openRepo/https://github.com/AlanSosa/Infinite-pew-pew-PC-Version) or [Mac](github-mac://openRepo/https://github.com/AlanSosa/Infinite-pew-pew-PC-Version) clients.
* Download as [zip](https://github.com/AlanSosa/Infinite-pew-pew-PC-Version/archive/master.zip) or [tar.gz](https://github.com/AlanSosa/Infinite-pew-pew-PC-Version/archive/master.tar.gz).

### License

Infinite pew pew is released under the [MIT License](http://opensource.org/licenses/MIT).

<a name="getting-started"></a>
![getting-started](http://s24.postimg.org/reobgq88l/getting-started-header.png "Getting Started")

Understanding the code is not gonna be a big deal, unless you reach the parts that I coded in a rush, anyway send me an e-mail everytime you feel confused, I'd be glad to help.

You can start by understanding the RIE ENGINE or go directly to the GAME CODE, both are in their respective folders. But for more information about RIE I recommend you to check out it's [repository](http://github.com/AlanSosa/Rie-Engine) where you can find related documentation.

Every class in the Game Code folder is documented, in order to know what an class does, just check the documentation that's inside of it. Every month I check the code and I do certain updates to the documentation, so make sure to check it out once in a while.

You can use whatever version of Visual Studio you're comfortable with, depending on your OS the setup is gonna be different but for that matter I recommend you to Google on how to Setup an XNA project for your OS and your Visual Studio version, because Microsoft is no longer supporting XNA. But that doesn't mean is totally dead, there's tweeks and "hacks" that talented programmers have done to make XNA work in newer versions of Windows, so don't be afraid to use Google. The magic words are " Install XNA on [Insert your OS version] ".

<a name="building"></a>
![building](http://s24.postimg.org/8fu5wbdbp/building-infinite-pew-pew-header.png "Building Infinite pew pew")

Infinite pew pew is provided with alredy compiled sources in the RELEASE folder. There are the .XNB's, to make Infinite pew pew run even without compiling it. After you downloaded it and setup the environment just click the RUN button and Infinite pew pew will compile and run. You will probably want to install before compiling:

- [Forced Square](http://www.dafont.com/forced-square.font)
- [Inversionz](http://www.dafont.com/inversionz.font)

In case I forgot to add the fonts needed, just check the .spritefonts files and in the <Font Name> mdownload the described fonts. Infinite pew pew includes the latest version of RIE, which allows you to modify, improve or tweek the game as you please without constraints, see the [Rie repository](http://github.com/AlanSosa/Rie-Engine) to find related documentation or check the source code inside the RIE ENGINE folder.

<a name="documentation"></a>
![documentation](http://s24.postimg.org/dig34ubzp/documentation-header.png "Documentation")

### I WILL NEED A HAND TO DOCUMENT THE PROJECT

Well, the project is not entirely documented, any help would be really appreciated, but for the parts that are documented, you can find it's documentation inside the source code. 

<a name="Requeriments"></a>
![requeriments](http://s24.postimg.org/jbq55enn9/requeriments-header.png "Requeriments")



<a name="contributing"></a>
![contributing](http://s24.postimg.org/6pb4utvsl/contributing-header.png "Contributing")

The [Contributors Guide](www.google.com.mx) contains full details on how to help with the development/documentation of Infinite pew pew. The main points are:

- Found a bug? Report it on [GitHub Issues][(www.google.com.mx) and include a code sample. Please state which version of Phaser you are using! This is vitally important.

- Pull Requests can now be made against the `master` branch (for years we only accepted PRs against the `dev` branch, but with the release of Phaser CE we've relaxed this policy)

- Before submitting a Pull Request run your code through [JSHint](http://www.jshint.com/) using our [config](https://github.com/photonstorm/phaser/blob/master/v2-community/.jshintrc).

- Before contributing read the [code of conduct](https://github.com/photonstorm/phaser/blob/master/v2-community/CODE_OF_CONDUCT.md).

Written something cool in RIE? Please tell me about it in alan.sosa.mejia@gmail.com

<a name="credits"></a>
![credits](http://s24.postimg.org/pvsxbr1o5/credits-header.png "Credits")

Infinite pew pew is a [Cubito Morado](http://cubitomorado.blogspot.com) production.

<img src="http://s15.postimg.org/beumtkhmz/logo_transpared_dark_label_590_256.png">

Created by [Alan Sosa](mailto:alan.sosa.mejia@gmail.com). Powered by lots of chocolate cookies and coffee.

- Music by [Georgy Chirkov](http://opengameart.org/users/gichco)
- "Crowd 1" sound effect [Phmiller42](http://freesound.org/people/phmiller42/)
- "Crowd 2" sound effect [Mlteenie](http://freesound.org/people/mlteenie/)
- "Crowd 3" sound effect [Zott820](http://freesound.org/people/zott820/)
- Some of the sound effects [Virix](https://soundcloud.com/virix)

"Above all, video games are meant to be just one thing: fun. Fun for everyone." - Satoru Iwata

### Aditional links 

[Landing Page Cubito Morado](http://www.cubitomorado.blogspot.com)
[Landing Infinite pew pew](http://cubitomorado.blogspot.com/p/infinite-pew-pew.html)
[Infinite pew pew on Windows Phone](http://www.microsoft.com/es-mx/store/p/infinite-pew-pew-free/9nblgggzmvvn)

