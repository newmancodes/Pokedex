# Pokemon API

Once you have cloned the repository using [Git](https://git-scm.com/downloads), and installed [Docker](https://docs.docker.com/get-docker/) you can cd into the root on the repository and execute:

> docker-compose up --build pokedex

This will pull the required images and start both [Jaeger](https://www.jaegertracing.io/) and the pokedex api.

You can navigate to Jaeger at [http://localhost:16686/search](http://localhost:16686/search) and you can send requests such as

> curl http://localhost:5000/pokemon/mewtwo

> curl http://localhost:5000/pokemon/translated/pikachu

by using [curl](https://curl.se/) or other similar http cli tools.

Once you are done, you can stop the containers via

> docker-compose down

For a developer focussed experience, you can instead

> docker-compose -f docker-compose-developer.yml up

This will stand up Jaeger as before, but also pull [Wiremock](http://wiremock.org/) to provide a disconnected environment.

Once you have installed the [.NET 5 SDK](https://dotnet.microsoft.com/download), you can execute the tests via

> dotnet test

And if you wish to open the code with a suitable IDE such as [Visual Studio 2019](https://visualstudio.microsoft.com/vs/) or [JetBrains Rider](https://www.jetbrains.com/rider/), you can then view the code.

Once you are done, you stop the containers via

> docker-compose -f docker-compose-developer.yml down