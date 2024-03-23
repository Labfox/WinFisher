<div align="center">
<img src="tmp/logo.svg" alt="Logo" width="80" height="80">

<h3>WinFisher</h3>

<p>Fake Window 10 Login Screen For Fishing</p>
</div>

![Project Demo](tmp/output.gif)

<!-- TABLE OF CONTENTS -->
<details open>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#background">Background</a></li>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#compiling">Compiling</a></li>
        <li><a href="#note">Note</a></li>
      </ul>
    </li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#screen-shots">Screen Shots</a></li>
  </ol>
</details>

## About The Project

### What Is It

This project presents a deceptive tool, a fake Windows 10 login screen, that enables attackers to display a counterfeit login interface on a victim's device. Its primary purpose is to gather login credentials from unsuspecting users. While its application is limited, certain scenarios, like the one discussed below, may necessitate its use.


### Background and UseCase Scenario

During my 3rd semesters at university, I discovered a vulnerability within our institution's web portal. Upon reporting it to the Head of Department and Vice Chancellor, I was granted permission to conduct penetration testing across all university systems.

Our university relies on MS Active Directory for authentication, with both faculty and students possessing distinct AD accounts. In lecture rooms, faculty members typically log into designated systems to access the web portal for managing attendance, quizzes, assignments, and results. Students are restricted from using these systems, except for occasions where they need to demonstrate projects or give presentations. In such cases, teachers often leave their accounts logged in and allow students to use them.

To exploit this scenario, I developed this application, designed to harvest teacher credentials, granting access to their web portals. After obtaining these credentials, This application just stores the credential in current directory but my variant o either uploaded creds to a publicly accessible smb share (accessible by both faculty and students), or stored them in a Firebase database. Moreover, my variant used different techniques for persistence and scheduling to evade detection.

During presentations, I discreetly installed this application on teacher systems, subsequently gaining access to their portals.

This project underscores the importance of cybersecurity awareness and the need for robust measures to safeguard against malicious attempts.

### Built With

![.NET](https://img.shields.io/badge/.NET-%235C2D91.svg?style=for-the-badge&logo=.net&logoColor=white)

![C#](https://img.shields.io/badge/C%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)

![Windows](https://img.shields.io/badge/Windows%20Presentation%20Foundation-0078D6?style=for-the-badge&logo=windows&logoColor=white)

 



## Getting Started

### Prerequisites

* Windows
* Visual Studio
* Window Presentation Foundation

### Compiling

1. Clone the repo
2. Open 'WinFisher.sln' in visual studio
3. Change mode to Release Mode because Debug mode does not block window hot keys
4. Build solution 


### Active Directory Environment
make following changes according to your target

Normal Account / Local Account:
![Normal Account](tmp/LA.png)

AD Account / Domain Account: 
![Domain Account](tmp/DA.png)
### Note

1. When compiled in DEBUG mode, window hotkeys are not disabled
2. When compiled in Release mode, window hotkeys i.e Win Key, Alt+Tab, Alt+F4 etc are disabled 

## License

Distributed under the MIT License. See `LICENSE` for more information.

## Contact

[![Twitter](https://img.shields.io/badge/Twitter-1DA1F2?style=for-the-badge&logo=twitter&logoColor=white)](https://twitter.com/BakarAamir)

[![LinkedIn](https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/bakar-git/)
