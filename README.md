# AdvancedEditor
A Mario Kart: Super Circuit track editor using MonoGame and ImGui

for use with a rom with a sha1 hash of `9d327c030c3e2d9007990518594f70c3340ac56f`

### Planned Features
- [ ] Import custom tilesets
- [ ] Custom Panorama background
- [ ] Change tracks
- [ ] Handle animated tiles
- [ ] Replace Track Names
- [ ] Replace Track Pictures
- [ ] Upcoming turn sign editor
- [ ] Fill / Reset Track
- [ ] Listen for more ideas!

# Building
**If you want to build a release build use the command in Release.txt and replace PLATFORM with the listed platform options**
## Windows
Requirements:
 - Visual Studio 2022

Clone or download the repository and open the `.sln` file in visual studio. Open the "configure startup properties" menu.
![image](https://github.com/aplerdal/AdvancedEdit/assets/59904070/82d43656-c483-48a2-bfbb-462c566e53aa)\
Change the startup project to AdvancedEdit\
![image](https://github.com/aplerdal/AdvancedEdit/assets/59904070/24a26863-0cc6-4be5-92eb-abbe3707a426)\
Run the project with F5 or by clicking the start button.
## Linux
Requirements:
 - Dotnet CLI
 - Dotnet SDK 8.0

Run the following commands in a terminal:
```bash
$ git clone https://github.com/aplerdal/AdvancedEdit.git
$ cd AdvancedEdit/AdvancedEdit
$ dotnet run
```
