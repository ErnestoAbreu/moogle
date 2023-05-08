# Moogle

![](moogle.png)

> Proyecto de Programación I.  
> Facultad de Matemática y Computación - Universidad de La Habana.  
> Curso: 2023.  
> Grupo: C-121.  
> Ernesto Abreu Peraza.

Moogle! es una aplicación *totalmente original* cuyo propósito es buscar inteligentemente un texto en un conjunto de documentos.

Es una aplicación web, desarrollada con tecnología .NET Core 6.0, específicamente usando Blazor como *framework* web para la interfaz gráfica, y en el lenguaje C#.
La aplicación está dividida en dos componentes fundamentales:

- `MoogleServer` es un servidor web que renderiza la interfaz gráfica y sirve los resultados.
- `MoogleEngine` es una biblioteca de clases donde está... ehem... casi implementada la lógica del algoritmo de búsqueda.

## Correr y usar el proyecto

Para correr el proyecto debes usar el comando `dotnet watch run --project MoogleServer` en Windows y `make dev` en Linux. En la carpeta `Content` deberán aparecer los documentos (en formato \*.txt) en los que el usuario va a realizar la búsqueda. En la casilla donde aparece *Introduzca la búsqueda* el usuario va a escribir que desea buscar y basta con apretar el botón *Buscar* para que Moogle! haga su trabajo.

## Arquitectura del proyecto

Cuando el programa empieza a correr se ejecuta el método `Compute()` de la clase `TF_IDF` del archivo `TF_IDF.cs` el cual precalcula información antes de que inicie la aplicación que será usada para el funcionamiento de la misma (leer documentos, calcular `TF_IDF`). Term frequency – Inverse document frequency (`TF_IDF`) es una medida numérica que expresa cuán relevante es una palabra para un documento en una colección de documentos.

Luego que la aplicación inicie, cuando se realize una consulta (*búsqueda*), el proyecto llama al método `Query` de la clase `Moogle` del archivo `Moogle.cs` donde se procesa la consulta y se compara con cada documento dandole una puntuación a cada uno y devolviendo una lista con los nombres de los documentos que más relevancia tienen, y también un fragmento por cada documento donde se puede apreciar en este una relación del documento con la consulta. La relevancia es calculada a partir de un modelo de espacio vectorial que se basa en el grado de similaridad de una consulta dada por el usuario con respecto a los documentos de la colección cuyos términos fueron ponderados mediante `TF_IDF`. Este consiste en que mientras mas pequeño sea el ángulo entre el vector de la consulta y el vector del documento, más parecidos van a ser estos.

Otra funcionalidad del Moogle! es que dará una sugerencia de búsqueda en caso de que el usuario *quizás* cometió un error al escribir la consulta. Para esto usamos un algoritmo de *Edit Distance* conocido como *Levenshtein distance* y a este costo le dividimos por el *Longest Common Prefix*. El propósito de este último proviene de la idea de que es más probable equivocarse en las últimas letras que en las primeras. Así `distra` está más cerca de `dijkstra` que de `citra`.

##

Para obtener una información más detallada y profunda de la arquitectura del proyecto, leer `Informe.pdf`.
