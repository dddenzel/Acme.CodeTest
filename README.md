# Code test for Evolve software
This is a code sample for Evolve software, as requested.  

It attempts to demonstrate a number of concepts:  
* Dependency injection
* Code reuse
* A clean sensible way to add future implementation for new print distributors per requirements
* Unit and integration testing
* A pipeline to handle high scale
* A pipeline that has failure isolation and message replayability.
* A pipeline that is reusable, triggered by either a monlty scheduled event or by an external system

# Data model
The data model is included as an entity relationship model. Can be viewed in visual studio code using the ERD Editor extension or as an image

# Overall design
The overall design including descriptions can be viewed here.

# Notes
The code is just a sample. It is perhaps a bit untidy/verbose for my liking - I tried to keep within the required time limits.
The data model is debatable. Its probably something close to what I would use in production for a relational database design - though strictly speaking it would allow a customer to use a print distributor from a different country. You would restrict this by both front end and back end validation.
  
