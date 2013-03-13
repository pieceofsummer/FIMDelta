# FIMDelta

Helper utility to selectively apply changes between developer and production FIM2010 servers. 

![screenshot](screenshot.png)

## Usage

After exporting configurations from both DEV and PROD servers with `ExportSchema.ps1`, and generating delta file with `SynchSchema.ps1`, you copy all 3 files (dev.xml, prod.xml and changes.xml) to the utility directory, and then run it.

Utility will show you all objects in delta as a tree. You can group it either by operation, or by object type.

Just check/uncheck objects or single attributes you want to export/skip, and press Save. It will create changes2.xml, which could be applied with `CommitChanges.ps1`.

The killer feature is reference tracking. If attribute is a reference, it will have referenced objects in a subtree. And if object is referenced by some attribute, it will have "Referenced by" node with all objects referencing it.
