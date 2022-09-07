# PichinchaDemoMicroservice
## Gustavo Alcívar

Para levantar la base de datos y el servicio, dirigirle a la raíz del proyecto y ejecutar:

<pre><code>docker-compose up
</code></pre>

Para ejecutar las pruebas de los endpoints, en otro terminal ejecutar:

<pre><code>docker build --target testrunner -t demopichincha .
</code></pre>

Y luego:

<pre><code>docker run --network=host demopichincha
</code></pre>