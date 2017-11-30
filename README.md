# Dir-Sync
###### Directory Syncronization Tool
Dir Sync is a convenient graphical tool for recursive directory tree and file differentiation & synchronization.
It's lightweight, fast and dead-simple to use. Dir Sync can be used for ie. keeping your flash drive files and it's backup copy in sync.
Since you may simultaneously work with both and change files (or create / delete directories or do any file system operation) keeping 
track of changes can be difficult.

### Warning
**Dir Sync is not a diff / merge tool based on file content. It will merge files _only_ by copying the newer file and overwriting the other.
Dir Sync can easily cause data loss if you're not careful. You have been warned.**

## Downloads
Pre-built binaries can be found inside [Releases](https://github.com/ahmetsait/Dir-Sync/releases) section.

## Usage
- Select which directories you want to check by clicking buttons [...] or you can just copy-paste paths inside text boxes as well.
- Click [Sync] and wait Dir Sync to traverse over the directory structures finding differences of the two.
- Upon finish, you will see a list consisting of directories and files.
- [Act] column indicates the suggested file / folder operation (action) for resolving the difference.
"-->" means copy to right, "<--" means copy to left, "X" means delete them both, and "●" means do nothing.
- You can change it by right clicking the items. It's also possible to change multiple actions at once by selecting more with either _Shift_ or _Ctrl_.
- Dir Sync can show both files / folders inside explorer (file manager) if you double click an item on the list.
- After changing 'actions' it's possible to merge the two directories by clicking [Bake].

## Limitations
- Dir Sync does not check file contents (in order to be fast). It can show you some false positives.
- Due to bugs in Windows (or whatever), Dir Sync ignores 1 minute difference between last modification times of files.

## License
Dir Sync is licensed under [The MIT License](LICENSE).
