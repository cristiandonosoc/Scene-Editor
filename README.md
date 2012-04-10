Scene Editor v0.1 (BETA)
========================

Este es el repositorio para un editor de escena que, si bien está pensado para usarse en la tarea 1, 
sirve para el resto del curso como referencia.

Controles
---------

Tomando como inspiración los programas previos a Y2K, la interacción con el usuario es poco intuitiva y comunicativa.
Los controles son los siguientes:

* wasd rota la cámara alrededor del target.
* WASD (mayúsculas) mueve el target. Este movimiento es dependiente de donde está la cámara (este es el único caso donde se buscó intuitividad).
* La rueda del mouse hace zoom de la cámara. Si no tienen rueda, el zoom tendrá que hacerse con los comandos anteriores (prioridad a cambiar).
* Click en una figura la selecciona y muestra las propiedades editables de este.
* Apretar en uno de los "botones" entra al modo de edición, donde se pide que se ingrese uno a uno los valores necesarios.
* En edición, TAB mantiene el valor pre-existente.
* ESCAPE sale de un modo que requiera input.
* c pone el modo creación, donde se puede agregar figuras. Por ahora solo se pueden crear triángulos y esferas.
* c+t+ENTER crea un triángulo.
* c+s+ENTER crea una esfera.
* ESPACIO exporta la escena.

Consideraciones
---------------

* Este programa está en estado BETA. Es posible que tenga bugs y errores.
* Si bien está pensado para ayudar a la generación de la escena, no es obligatorio su uso. No se aceptan excusas por mal funcionamiento del programa.
* El editor intentará importar una escena en el archivo "Scene/sceneInfo.xml"
* Si no hay archivo, o no hay elementos especificados (cámara, luces), el editor los creará por default.
* El editor no cuenta con soporte de materiales. Si la escena importaba cuenta con materiales, el editor las exportará de vuelta.
* El editor crea por default un fondo negro y un material amarillo "Yellow". Todas las figuras tienen este material por default. El manejo de materiales requiere edición manual del XML.

Código Fuente
-------------

El programa está pensado para que sea visto por los alumnos de manera que puedan tener una referencia de como se hacen programas en OpenGL.
Debido a esta razón, el código del programa está comentado de manera extensiva, con tal de que sea fácil de comprender.
No obstante lo anterior, debido a la rapidez del trabajo (en especial en la última etapa del beta), los patrones de diseño han fallado un poco, al igual que los comentarios.
Se recomienda partir por el archivo Editor/Program.cs, que es donde está definido el main y la lógica principal del programa.

Cualquier detalle sobre el editor lo pueden comunicar a través de github, la página del curso o a cristiandonosoc@gmail.com

