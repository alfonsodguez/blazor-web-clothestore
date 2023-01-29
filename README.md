# blazor-web-clothestore
Aplicacion web: Comercio de ropa 

**ÍNDICE** 

1. [INTRODUCCIÓN .................................................................................................... 4 ](#_page3_x68.00_y70.92)
2. [OBJETIVOS ........................................................................................................... 4 ](#_page3_x68.00_y304.92)
2. [DESCRIPCIÓN DE LAS TECNOLOGÍAS UTILIZADAS......................................... 5 ](#_page4_x68.00_y70.92)
2. [FUNCIONALIDAD DEL PORTAL WEB .................................................................. 5 ](#_page4_x68.00_y445.92)
2. [DIAGRAMA DE BASE DE DATOS ......................................................................... 6 ](#_page5_x68.00_y321.92)
2. [DESARROLLO A DESTACAR ............................................................................... 7 ](#_page6_x68.00_y70.92)
1) [*TreeView*........................................................................................................... 7 ](#_page6_x68.00_y101.92)
1) [*Productos, tallas y stock* ................................................................................. 11 ](#_page10_x68.00_y70.92)
1) [*Uso de Entity Framework*................................................................................ 16 ](#_page15_x68.00_y521.92)
1) [*Control de stock y transacciones* .................................................................... 17 ](#_page16_x68.00_y611.92)
1) [*Badge* ............................................................................................................. 18 ](#_page17_x68.00_y331.92)
1) [*Alert tab y control de clics en botón comprar* .................................................. 20 ](#_page19_x68.00_y70.92)
7. [CONCLUSIÓN ...................................................................................................... 21 ](#_page20_x68.00_y70.92)
8. [WEBGRAFÍA ........................................................................................................ 22 ](#_page21_x68.00_y70.92)
1. **INTRODUCCIÓN** 

Lo que me llevó a decidir el enfoque de mi proyecto fue la idea de implementar el control de stock de un determinado producto. Entonces, ¿por qué hacerlo con ropa? La razón es que en clase trabajamos con  productos como libros y comida, más simples a la hora de manejar un control de stock, en cambio la ropa, al contar con una variedad de tallas por cada unidad de producto añade un plus de dificultad en la implementación al ser más factores a tener en cuenta. Por ello, me he inspirado en la web de[ zalando.es ](https://www.zalando.es/) a la hora de desarrollar y diseñar el portal. 

2. **OBJETIVOS** 

El principal objetivo de este proyecto ha sido poner en práctica los conocimientos adquiridos  durante  el  curso,  utilizando  tecnologías  vistas  en  clase  además  de aprovechar la ocasión para profundizar en desarrollos que no tuvimos la ocasión de implementar. 

A  continuación  enumero  las  funcionalidades  más  importantes  que  se  pretenden cumplir con la realización del proyecto: 

1. Control de *stock.* 
1. Uso de transacciones. 
1. *Treeview* dinámico y escalable.  
1. Almacenar objetos y listas en tablas de base de datos mediante *EF*. 
1. Uso de *state container* para la comunicación entre componentes. 
1. Uso de *templated component.* 
1. Sincronización entre los distintos elementos que conforman la aplicación. 
1. Uso de *modales* y *badge* de *Bootstrap.* 
3. **DESCRIPCIÓN DE LAS TECNOLOGÍAS UTILIZADAS**  

Para crear *Zalandú*, escogí la tecnología de *Blazor* apoyada en el *C#* y con escaso uso de JavaScript, únicamente para manejar en *localStorage,* y algún componente de *Bootstrap,* como los *alerts*, debido a la comodidad que proporcionan tanto el IDE propio  *VS*  como  la  potencia  *C#,*  facilitando  mucho  el  desarrollo.  Todo  ello  en *frontend*. 

En  *backend*  hago  uso  del  propio  servidor que  se  crea  por  defecto  al  iniciar  un proyecto *Blazor* en *VS*. 

La base de datos escogida es *SQL Server* que viene integrada con el *VS,* pero usando el *ORM*  *Entity Framework* para el acceso a datos.  

Para el diseño de las interfaces de usuario he utilizado *Bootstrap,* un *framework* *CSS.* 

4. **FUNCIONALIDAD DEL PORTAL WEB** 

La funcionalidad de este es sencilla. Para comprar ropa se requiere de un registro previo. Yo he seguido la estrategia de permitir añadir productos al carrito sin la necesidad de estar *logueado* o registrado. Únicamente, se solicitará esta condición al cliente cuando trate de finalizar el pedido, ahí es cuando compruebo si el cliente está  *logueado*  o  no.  En  caso  afirmativo,  continua  el  proceso  para  completar  la compra si no, le redirecciono a la vista de *login*, que además cuenta con un botón de registro para que el usuario cumplimente el pertinente formulario. Una vez que el usuario esté *logueado*, aparecerá una vista donde se muestran los datos de envío del cliente; un formulario con campos requeridos que una vez cubierto permitirá finalizar el pedido. 

La idea es ir recabando los datos del cliente por etapas, primero en el registro y después en la finalización del pedido, creando así un registro rápido y sencillo para el cliente. 

Otra estrategia que he llevado a cabo es permitir que solo se pueda comprar un máximo de tres unidades por talla de cada producto, salvo en los casos en los que el stock sea inferior a esta cifra, entonces, la cota se pondrá en el valor del stock.  

Si un usuario trata de comprar un número mayor a estas dos cotas, se mostrará un *tab* o etiqueta de alerta; para ello use el componente *alert* de *Bootstrap*. 

5. **DIAGRAMA DE BASE DE DATOS** 

Para la creación del diagrama de base de datos instalé las dependencias de *Visual Studio Build Tools*. En la siguiente captura se puede observar como solo represento aquellas tablas necesarias para el funcionamiento de mi aplicación, por tanto, omito las que se crean por defecto con *EF*. ![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.001.png)

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.002.jpeg)

6. **DESARROLLO A DESTACAR** 
1) ***TreeView*** 

El *treeview* está pensado para ser escalable, independientemente de las categorías o subcategorías que se le puedan añadir en un futuro desarrollo, se van a mostrar correctamente.  

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.003.jpeg)

**Ilustración 1: selección de la categoría “padre” “Ropa”** 

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.004.jpeg)

**Ilustración 2: selección de la categoría “hijo” “Camisas”** 

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.005.jpeg)

**Ilustración 3: selección categoría “nieto” “Camisas informales”** 

En las imágenes se puede apreciar como al seleccionar las distintas categorías y subcategorías,  la  lista  de  productos  va  variando,  manteniéndose  aquellos  que cumplen con el filtro. 

Este  es  el  bloque  de  código  con  más  algoritmia  de  toda  la  aplicación.  Para  la creación me he servido de un componente “recursivo” que he llamado “RecursiveUI".  Es un componente  que se invoca a sí mismo desde dentro del propio componente. Las variables intermedias ayudan a controlar que no se pinten todas las categorías y subcategorías  de  una  vez,  controlando  los  eventos  del  ratón  para  expandir  y colapsar el *treeview*.  

Vayamos al código: 

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.006.jpeg)

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.007.png)

Estos dos componentes conforman el *treeview*.  

El componente “padre*”*, “PanelCategorias”, le pasa la lista de categorías que se ha recuperado de la base de datos, además del identificador, al componente hijo. Este último,  usará  el identificador  para  saber  por  qué  categoría  tiene que  empezar  a “pintar”. 

En este caso, el identificador será el valor 0 y se corresponderá a las categorías raíz: ropa, deporte, zapatos y complementos. Una vez encontrada la primera categoría, busca todas aquellas subcategorías “hijas*”* que coincidan con el identificador de esta categoría raíz y lo mismo pasaría con las categorías “nietas”, solo que cambiamos la condición anterior  por el identificador de las categorías “hijas”. 

2) ***Productos, tallas y stock***  

Así se vería en la aplicación la vista con los detalles de un producto cualquiera:

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.008.jpeg)

El contenido de la vista es dinámico, desde la foto, la marca de ropa, el precio, hasta las tallas con su correspondiente precio y stock, del que solo muestro un mensaje de aquellas tallas que tengan un número de stock inferior a tres, y el contenido del componente *accordion* de *Bootstrap.*  

Vayamos por partes:  

1) Tablas 

Primero veremos cómo está almacenado el contenido dinámico en la base de datos. 

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.009.jpeg)

**Ilustración  4:  extracto  tabla  Productos.  Señalado  en  azul  el  producto  mostrado  en  la  vista** 

**anterior.** 

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.010.jpeg)

**Ilustración 5: tabla con el stock de cada talla del producto.** 

Un  producto  está  compuesto  por  varias  tallas  -  pueden  ser  de  dos  tipos: alfanuméricas o numéricas, aunque realmente son *string*-. Ahora bien, cada talla-

producto ha de ser diferenciado ya que tienen un precio y stock distintos por eso, necesitamos ambas tablas.  

En  la  primera  cada registro único  corresponde  a  un  producto,  en  cambio,  en la segunda,  cada  registro  se  diferencia  por  una  clave  primaria  compuesta  por  los campo “ProductoId-Talla”. Esto es importante para llevar el control de stock que más adelante explicaré. 

2) Controlador 

El siguiente extracto de código muestra como enlazo los datos de ambas tablas almacenando  en  cada  producto  que  recupero  de  la  base  de  datos  sus correspondiente tallas con stock que luego pintaré en la vista.   

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.011.jpeg)

**Ilustración 6: bloque de código en 2ClienteController”** 

Esto es posible añadiendo una propiedad llamada “Stock” que he definido en el modelo Producto. Es una lista de objetos “StockProducto” que no se va a mapear contra la base de datos, pero nos permite tener toda la información en un único objeto.  

3) *State container* 

Otro criterio que he seguido es recuperar todos los productos de una tacada para no tener que hacer más peticiones al servidor cada vez que un usuario quiera ver la información detallada de un producto. Esto es posible al  utilizar la comunicación entre componentes. Para este caso en concreto me apoyo en un *state container*. 

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.012.jpeg)

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.013.jpeg)

Como podemos observar en el código, en el “StateContainerProducto” indicamos que componentes queremos que tengan acceso a los métodos del servicio.  

Veamos su uso en el componente:  

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.014.jpeg)

Este componente “hijo*”* recibe un objeto Producto pasado por el componente “padre” en cascada. A continuación, se envía al *state container* por medio de uno de sus métodos “PassProducto” que ha sido inyectado. Una vez “almacenado” en el *state container,* podemos recuperarlo en otros componentes. No obstante, solo pueden ser aquellos que he mencionado con anterioridad.  

El código para recuperar esos datos se ve da la siguiente forma: 

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.015.png)

4) *Dropdown* tallas  

Para  el  *dropdown*  he  requerido  del  uso  de  un  *templated  component.*  Su implementación se ve de la siguiente manera: 

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.016.png)

**Ilustración 7: fragmento de código del componente DetallesProducto** 

Este es como un mini bloque *html,* pero dinámico que se incrusta en la vista del componente. Así se vería implementado en la vista: 

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.017.png)

**Ilustración 8: fragmento de código vista DetallesProducto** 

3) ***Uso de Entity Framework***  

Para  la  creación  de  las  tablas  me  he  servido  de  *EF*,  lo  que  me  ha  permitido almacenar  tanto  objetos  como  colecciones  en  la  base  de  datos,  todo  ello serializados.  

La implementación para almacenar y recuperar listas es mucho más compleja que para objetos. Como podemos observar en la siguiente captura: 

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.018.jpeg)

En la base de datos lo almacena serializado como un *JSON* tal como se muestra aquí: 

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.019.png)

Con objetos es más sencillo: 

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.020.png)

En la tabla se vería así: 

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.021.png)

4) ***Control de stock y transacciones*** 

Una vez que el usuario finaliza la compra, se realiza una actualización de las tablas que  intervienen  en  el  proceso:  “PedidoCliente”,  “ItemsPedido”,  “StockProducto”. Como hay que lanzar varias operaciones de escritura sobre la base de datos, hago uso de las *transacciones* para que se grabe en todas las tablas haciendo *commit,* o 

en ninguna, con un *auto*-*rollback* en caso de una no deseable circunstancia externa. 

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.022.jpeg)

**Ilustración 9: fragmento de código del “ClienteController”** 

Simulo un *trigger* ya que *EF* no los “dispara”.  

5) ***Badge***  

El *badge* es un componente de *Bootstrap* que uso para informar al usuario de que el producto ha sido añadido a la cesta además de informar de la cantidad total de estos.  

Aquí una comparativa visual del icono de la cesta sin compra y con la cesta llena. 

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.023.png)

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.024.png)

**Ilustración 10: *header* del *layout*** 

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.025.jpeg)

**Ilustración 11: fragmento código *html* del *Layout*** 

Lo más importante de esta implementación es que intervienen varios componentes: “\_Layout”, “DetallesProducto”, “MostrarPedido”.  

Todo esto ha de ser sincronizado para que la información sea consistente allá donde se  use.  Para  lograrlo,  hago  dos  cosas:  usar  el  *state  container*  -  necesario  sino perderíamos estos datos temporales- y pasar en cascada el *layout* -eso nos permite tener acceso a los métodos implementados en este-. 

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.026.png)

El uso del objeto *\_\_Layout* en un componente se haría de la siguiente manera:  

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.027.png)

6) ***Alert tab y control de clics en botón comprar***  

El componente *alert* de *Bootstrap* se “dispara” cuando un usuario infringe la regla de negocio  que  he  mencionado  anteriormente.  Para  ello  hago  un  seguimiento  del número de clics que se hacen por talla de producto. 

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.028.png)![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.029.png)

Aquí  sí  que  he  necesitado  usar  código  JavaScript  para  mostrar  y  cerrar  esta etiqueta. Esto es posible gracias al mecanismo de interoperabilidad *JSRuntime* que provee *Blazor*. 

![](Aspose.Words.a40b87a9-766e-4dd0-bc1a-62f5e3c27ee2.030.png)

Llamo  a  mi  función   JavaScript  “alert”.  Esta  es  una  función  que  admite  tres parámetros: el mensaje de alerta que quiero que aparezca en la vista, el tipo de componente  *alert,*  en  mi  caso  *danger,*  y  por  último,  si  lo  quiero  hacer  visible  u ocultarlo. 

7. **CONCLUSIÓN** 

La creación de la web *Zalandú* ha sido el resultado de la investigación y uso de las múltiples herramientas que se nos han ido aportando a lo largo del curso. A través de dicho proyecto, he podido comprobar con mayor detenimiento las dificultades que pueden acarrear la programación y la paciencia necesarias para solventar cualquier problema, o en este caso, error. No obstante, uno no se dedica a la programación, como ya dijo John F. Kenny en su día: *“[…] porque sean metas fáciles, sino porque son difíciles.”* Por ello, aprecio todos los conocimientos que he logrado adquirir a partir de un proyecto como este. 

8. **WEBGRAFÍA [https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-6.0 ](https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-6.0)[https://stackoverflow.com/ ](https://stackoverflow.com/)**

[https://www.zalando.es/ ](https://www.zalando.es/) 
22 
