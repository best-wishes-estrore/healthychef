3/9/2010 Jerry Woodlock
Upgrades to FCKEditor, CKFinder
---------------------------------------------------------------
FCKEditor upgraded from 2.6.3 to 2.6.6
CKFinder upgraded from 1.2.3 to 1.4.3

Upgrade of CKFinder required to cope with Firefox 3.6 issue 
preventing viewing of files in filemanager and 'Browse Server' command
from within WYSIWYG content editor.

Manual upgrade instructions
---------------------------------------------------------------
1-Replace from //tfs/bayshore solutions/webmodules/branches/webmodules-2009.8.8.x-maintenance/webmodulescmstemplate/
a. /WebModules/Components/TextEditor 
b. /WebModules/Components/ImagePicker
c. /WebModules/FileManager

2-Remove reference to CKFinder, FredCK.FCKeditorV2
3-Add references to 
a. /WebModules/Compnoents/TextEditor/lib/CKFinder.dll
b. /WebModules/Compnoents/TextEditor/lib/FredCK.FCKeditorV2.dll
c. .NET assembly System.Design

3-when preparing release package, remember to include the same folders of code, as well as the updated dlls