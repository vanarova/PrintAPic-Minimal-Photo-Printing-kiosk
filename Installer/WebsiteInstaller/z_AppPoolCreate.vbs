Set adminManager = CreateObject("Microsoft.ApplicationHost.WritableAdminManager")
adminManager.CommitPath = "MACHINE/WEBROOT/APPHOST"
Set applicationPoolsSection = adminManager.GetAdminSection("system.applicationHost/applicationPools","MACHINE/WEBROOT/APPHOST")
Set applicationPoolsCollection = applicationPoolsSection.Collection

Set addElement = applicationPoolsCollection.CreateNewElement("add")
addElement.Properties.Item("name").Value = "PrintAPicPool"
addElement.Properties.Item("autoStart").Value = True
addElement.Properties.Item("managedPipelineMode").Value = "Integrated"
addElement.Properties.Item("startMode").Value = "AlwaysRunning"


Set processModelElement = addElement.ChildElements.Item("processModel")
processModelElement.Properties.Item("identityType").Value = "LocalSystem"
processModelElement.Properties.Item("idleTimeout").Value = 240

Set recyclingElement = addElement.ChildElements.Item("recycling")
Set periodicRestartElement = recyclingElement.ChildElements.Item("periodicRestart")
Set scheduleCollection = periodicRestartElement.ChildElements.Item("schedule").Collection
Set addElement1 = scheduleCollection.CreateNewElement("add")
addElement1.Properties.Item("value").Value = "12:00:00"
scheduleCollection.AddElement(addElement1)


applicationPoolsCollection.AddElement(addElement)
adminManager.CommitChanges()