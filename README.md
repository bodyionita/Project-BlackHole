# Android VR - BlackHole [![Build Status](https://travis-ci.com/bodyionita/Project-BlackHole.svg?branch=master)](https://travis-ci.com/bodyionita/Project-BlackHole)

*This is an industry project on which my dissertation is based on. The paper can be found [***HERE***](https://drive.google.com/file/d/1EaVCF7VU8gH9VLK3lrNfQ-2uwO1yFhPz/view?usp=sharing).
The project development ran from :calendar: __09/2018__ to __04/2019__. :calendar:*

## About :information_source:

The project **BlackHole** is a holographic galaxy built around a black hole where all the stars represent 
specific public companies. The relationship between stars and the black hole, their shape 
and color is defined by stock price data such as volume, price and time.

The aim of this project is to transform raw data sets into an interactive and immersive experience by 
visualising everything in Virtual Reality through the use of an Android mobile phone and a VR headset like
[Google Cardboard](https://vr.google.com/cardboard/)


## Getting Started

### Data

In order to get the data required for this project, please visit [Project BlackHole Data Gathering](https://github.com/bodyionita/Project-BlackHole-DataGathering) repo.

### Unity

1. The project was build using Unity3D version 2018.3.7f. [Download](https://unity3d.com/unity/whats-new/2018.3.7) the Unity Editor and Android Targe Support for you OS (Windows/MacOS) and install them.
2. Get git
3. Clone this repository by `git clone https://github.com/bodyionita/Project-BlackHole.git`
4. Open in Unity the project with the root in `BlackHole` folder
5. Build the project and export as an `.apk` which can then be installed on an Android phone.

### Configurations
1. `Assets/Scripts/Data/StreamRequest.cs:16` - change the connection string to the one to your database 
2. `Assets/Scripts/Planet/Planet.cs:8-18` - change to limits of the planets visual configurations
3. `Assets/Scripts/Simulation/SimManager.cs:30-31` - change how many seconds one day of simulation takes
4. `Assets/Third Party/loadingBar/scripts/loadingtext.cs:12` - change how many seconds the loading screen should take (based on how much pre streaming you want the application to do
5. `Assets/UI/TooltipArrow.cs:29:31` - parametres of the tooltip arrow which appears when hovering a planet


## Contributors :pencil2:

- **[Bogdan Ionita](https://www.linkedin.com/in/bionita/)** - Author 

    I am a 4th year Computer Science student at [University College London](https://www.ucl.ac.uk/) 
    and have chosen to do this industry project as basis for my dissertation.

    :e-mail:  bogdan.ionita96@gmail.com

    :e-mail:  bogdan.ionita.15@ucl.ac.uk
    
- **[Rae Harbird](http://www.cs.ucl.ac.uk/people/R.Harbird.html/)** - Project Supervisor
    
    Rae is currently a teacher at [University College London](https://www.ucl.ac.uk/) and has supervised
    my entire development and progress through the academic year.
    
- **[Amin Naaj](https://www.linkedin.com/in/amin-n-87375b116/)** - Industry Liaison

    Amin is currently an employee of [Algoraise](https://algoraise.com/) which came up with the idea
    and the motivation for this project, and was my guide towards achieving something meaningfull.

## License

This project is licensed under the GNU General Public License v3.0 - see the [LICENSE](LICENSE)
file for details
