# Simple Windows Shell
**SWH** is a console applications written in **C#** that emulates Linux-like command behavior on Windows.
## Usage
### Login
**LoginScreen()** function is used when signing onto a "system". To use the application user must first login. At login, user needs to provide his *Username* and *Password*. **LoginScreen()** prompts for the *Username*. The user is then prompted for a *Password*, where appropriate. Echoing is disabled to prevent revealing the *Password* and its length.

The application will check if such user exists in databese **Users.db**. If user exists and the *Password* is correct, user's "shell" is started, otherwise following is writtien "*Login incorrect*" to the console and the user is then prompted for the *Username* again. The whole proccess is made to mirror Linux-like behavior with *init* and *login* process.

### Shell
Once user has succesfully logged in, he will be in **SWH** shell. Shell will prompt user for a new command. Commands *addUser* and *rmUser* can only be used by **root** user.

* find - print lines that match pattern with a line number
  * find "*pattern*" path 
* findDat - search for files in a directory hierarchy
  * findDat *filename* *path*
* clear - reset the terminal
* alias - customize "bash prompt"
  * alias
  * alias *newaliasname*
* exit - cause user's shell termination
* logout - logout and return to the "*login* process" (**LoginScreen()**)
* addUser - create a new user
* rmUser - delete a user account and related files

### Database
The database contains 3 users that have equivalent *Username* and *Password*.
| N. | Username | Password |
| ------- | ------- | ------- |
|1.       | aleksa  | aleksa  |
|2.       | majkic  | majkic  |
|3.       | root    | root    |

Also, here's the property query for my SQLite database:
~~~~
CREATE TABLE Users (
    Id                INTEGER   PRIMARY KEY AUTOINCREMENT
                                UNIQUE,
    Username          CHAR (20) NOT NULL
                                UNIQUE,
    Salt              BLOB,
    PassHash          BLOB,
    LastLogin         CHAR (50)
);
~~~~

## Commands
Detailed explanation of the individual commands. All commands are based on the real Linux commands and they are simplified.
COMMAND | NAME | SYNOPSIS | DESCRIPTION | OPTIONS | LINUX COUNTERPART|
| --- | --- | --- | --- | :---: | --- |
| WHERE | where - print the name of the current/working directory | **where** | Print the full filename of the current working directory. | x | Command **pwd** is the Linux counterpart. |
| GO | go - change the working directory | **go** [*path*]<br/>**go** .. | Change the working directory of the current "shell execution environment".<br/><ul><li>If no *path* operand is given, error message "*The command or its signature is wrong.*" will be displayed.</li><li>If the *path* operand is dot-dot, the current path will be changed to the previous subdirectory.</li></ul> | x | Command **cd** is the Linux counterpart. |
| CREATE | create - create a directory or a file | **create** [*-d*] *path* | Create the directorie or a file specified by the operand. If no option is used, command *create* will create a new file. If no *path* operand is given, error message "*The command or its signature is wrong.*" will be displayed. | **-d** *path* Create a directory at a speciefied path. | Command **mkidr**/**touch** is the Linux counterpart. |
LIST | list - list directory contents | **list** [*path*] | List object names (files and directories) alphabetically. If no *path* operand is given, current directory content will be displayed. | x | Command **ls** is the Linux counterpart. |
PRINT | print - print txt file on the standard output | **print** [*path*] | Print a file with extensions.<br/><ul><li>If no *path* operand is given, error message "*The command or its signature is wrong.*" will be displayed.</li><li>If file has no extension, error message "*File **filename** doesn't have extension information.*" or "*File doesn't have an extension.*" will be displayed.</li><li>If file isn't a text file, error message "*File **filename** is not a text file.*" will be displayed.</li><li>If file doesn't exist in the given path, error message "*File **filename** doesn't exist in path **path**.*"</li></ul> | x | Command **cat** is the Linux counterpart. |
