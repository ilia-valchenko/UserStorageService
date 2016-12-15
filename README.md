# Master-slave replication & WCF
 - Master and slaves are communicate by using NotificationMessage class. 
 - Slaves take a part of work of master such as searching users by the given criteria. 
 - If a slave doesn't find the user in a local storage it sends a message to the master. The user which will be received from the master will be put into a local storage (It looks like a simple cache.).
