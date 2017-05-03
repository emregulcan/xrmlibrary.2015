# xrmlibrary.2015
Microsoft Dynamics CRM 2015 helper library

This project includes 2 helper library for Microsoft Dynamics CRM 2015.
* **XrmLibrary.EntityHelpers** : Microsoft Dynamics CRM 2015 Entity Helper. This library includes almost full MS CRM 2015 SDK methods and UI methods.
* **XrmLibrary.ExtensionMethods** : Microsoft Dynamics CRM 2015 SDK extension methods

For more information please visit https://github.com/emregulcan/xrmlibrary.2015/wiki

If you're having trouble please visit https://github.com/emregulcan/xrmlibrary.2015/issues

Your comments and pull requests are very important and always welcome


# Commit / Release Notes

## v1.1
- [GetMemberList](https://github.com/emregulcan/xrmlibrary.2015/blob/master/XrmLibrary.EntityHelpers/Marketing/ListHelper.cs#L281-L308) method added to Marketing.ListHelper. This retrieves all members in marketing list (without regarding Static or Dynamic) and returns ListMemberResult class. 
- [BaseEntityHelper.Get(string fetchxml)](https://github.com/emregulcan/xrmlibrary.2015/blob/master/XrmLibrary.EntityHelpers/Common/BaseEntityHelper.cs#L101-L107) mofied to use "FetchExpression".
- [XrmLibrary.ExtensionMethods.2015](https://github.com/emregulcan/xrmlibrary.2015/tree/master/XrmLibrary.ExtensionMethods) assembly dependecy removed.

## v1.0
- First release
