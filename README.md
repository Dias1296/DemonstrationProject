# DemonstrationProject
ASP.NET Core Web API with Onion Architecture Implementation. Developed as a way to demonstrate several concepts used in building web APIs.


->	Onion Architecture implementation with separation of concerns in the following layers: Domain, Service, Infrastructure and Presentation.
	Flow of dependencies is towards the core(Domain) of the onion. Lower levels are used to define contracts and interfaces. Higher layers used for infrastructure or external services.

->	Logging service using a singleton implementation.

-> 	Handling of HTTP requests. Including the use of DTOs and validation of requests.

-> 	Global error handling with the middleware.

-> 	Parent/child relationships in an API through the Company/Employees dynamic.

-> 	Asynchronous programming.

-> 	Application of several request modifiers, such as paging, filtering, searching, sorting and data s

->	Implementation of HATEOAS, adding links to requests to aid in API navigation.

-> 	Creation of a root document so API is more traversable.

-> 	Versioning demonstration with controllers.

-> 	Caching (expiration and validation model).

->	Rate limiting.

->	User authentication using JWT and refresh tokens.

->	Swagger for documentation.