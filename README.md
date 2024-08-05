# Code test for Evolve software
This is a code sample for Evolve software, as requested.  

It attempts to demonstrate a number of concepts:  
* Inversion of control.  
* Code reuse and a clean sensible way to add future implementation for new print distributors per the requirements.  
* Unit and integration testing.  
* A pipeline to handle scale.  
* A pipeline that has failure isolation and message replayability.  
* A pipeline that can be used by a potential new crm or subscription management system.  

# Data model
The data model is included as an entity relationship model [here](https://github.com/dddenzel/Acme.CodeTest/blob/main/DataModel.vuerd.json). It can be viewed in visual studio code using the [ERD Editor extension](https://marketplace.visualstudio.com/items?itemName=dineug.vuerd-vscode) or as an [image](https://s3.ap-southeast-2.amazonaws.com/codetest.public/DbModel.png).  
  
This data I would expose over a simple REST API, with relevant search filters for active subscriptions based on a date range - seen [here](https://s3.ap-southeast-2.amazonaws.com/codetest.public/Api.PNG)

# Overall design
The overall design including descriptions can be viewed [here](https://s3.ap-southeast-2.amazonaws.com/codetest.public/CodeDesign.png).

# Notes
The code is just a sample. It is perhaps a bit untidy/verbose for my liking - I tried to keep within the required time limits.  
The data model is debatable. Its probably something close to what I would use in production for a relational database design - though strictly speaking it would allow a customer to use a print distributor from a different country. You would restrict this by both front end and back end validation.  
A monthly scheduled event might work for a single timezone, but if this software has to the potential to be across multiple timezones, a different approach may be needed. Possibly multiple triggers or some business logic within a single trigger.  
  
