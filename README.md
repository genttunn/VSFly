# VSFly
ASP.NET Core project (Microsoft EF, REST API and MVC client)


Introduction
Aircraft price management application for the airline VSFly

Contraints

For each flight available in the database, the other partner websites (ebooker / skyscanner type) can buy tickets for their customers through their websites as a front-end using webAPI requests from the BLL of their sites.

For each flight a base priceis offered by the airline. Rules exist to maximize the filling ofthe aircraft and the total gain on all seats. For this there are 2 variables (the filling rate of the plane and the deadline of the flight in relation to the date of purchase of the ticket). The calculation of the sale pricemust be done on the WebAPI server side and be returned to the partner site on each request. 

In the database managed by Entity Framework, the sale priceof each ticket must be saved.

    1.If the airplane is more than 80% full regardless of the date:sale price= 150% of the base price

    2.Ifthe plane is filled less than 20% less than 2 months before departure:sale price= 80% of the base price

    3.If the plane is filled less than 50% less than 1 month before departure:sale price= 70% of the base price

    4.In all other cases:a. sale price= base price

Delivery

The result consists of 2 Visual Studio solutions:

  1)Partner sitea.With an MVC presentation layer (.net core) for:
  
       i.List of flights
    
       ii.Buy tickets on available flights (no change or cancellation possible)b.With a business layer to communicate with the webAPI
    
  2)VSAFly's WebAPI
  
       a.With a webAPI layer.A controller accepting RESTfull requests and returning the data in JSON format.Requests to be processed:
       
              a.Return all available flights (not full)
              
              b.Return the sale priceof a flight
              
              c.Buying a ticket on a flight
              
              d.Return the total sale priceof all tickets sold for a flight
              
              e.Return the averagesale price of all tickets sold for a destination (multiple flights possible)
              
              f.Return the list of all tickets sold for a destination with the first and last name of the travelers and the flight number as well as the sale priceof each
              
              ticket.
              
       b.With an EntityFramework core layer to access the database as illustrated in the following figure.
       
       
       
       
