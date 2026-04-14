-- ============================================================
-- Seed de usuarios, hilos y respuestas
-- Usuarios tomados del ForoU, sin tildes para evitar encoding
-- El admin (Chris Beita) no crea ni responde hilos
-- ============================================================

-- Usuarios (mismos nombres que ForoU, password simple para pruebas)
-- ForoWeb ya tiene: 1=Chris Beita(Admin), 2=Estudiante Demo, 3=Profesor Demo
-- Estos quedan con IDs 4 en adelante
INSERT INTO Users (FullName, Email, PasswordHash, Role, IsActive) VALUES
('Jorge Salazar',    'jorge@ufide.ac.cr',            '123',  'Estudiante', 1),  -- 4
('Ana Maria Solis',  'ana.solis@ufide.ac.cr',         '123',  'Estudiante', 1),  -- 5
('Carlos Jimenez',   'carlos.jimenez@ufide.ac.cr',    '123',  'Estudiante', 1),  -- 6
('Daniela Rojas',    'daniela.rojas@ufide.ac.cr',     '123',  'Estudiante', 1),  -- 7
('Eduardo Mora',     'eduardo.mora@ufide.ac.cr',       '123',  'Estudiante', 1),  -- 8
('Fernanda Castro',  'fernanda.castro@ufide.ac.cr',   '123',  'Estudiante', 1),  -- 9
('Gabriel Vargas',   'gabriel.vargas@ufide.ac.cr',    '123',  'Estudiante', 1),  -- 10
('Hilda Nunez',      'hilda.nunez@ufide.ac.cr',        '123',  'Estudiante', 1),  -- 11
('Ivan Blanco',      'ivan.blanco@ufide.ac.cr',        '123',  'Estudiante', 1),  -- 12
('Juliana Perez',    'juliana.perez@ufide.ac.cr',      '123',  'Estudiante', 1),  -- 13
('Kevin Ulate',      'kevin.ulate@ufide.ac.cr',        '123',  'Estudiante', 0),  -- 14 (inactivo)
('Alvaro Miranda',   'alvaro@ufide.ac.cr',             '123',  'Profesor',   1);  -- 15

-- Profesores disponibles para responder:
--   UserId 3  = Profesor Demo
--   UserId 15 = Alvaro Miranda
-- Estudiantes disponibles:
--   4=Jorge, 5=Ana Maria, 6=Carlos, 7=Daniela, 8=Eduardo
--   9=Fernanda, 10=Gabriel, 11=Hilda, 12=Ivan, 13=Juliana

-- ============================================================
-- CATEGORIA 1: Bases de Datos
-- ============================================================
INSERT INTO Threads (CategoryId, UserId, Title, Message, CreatedAt) VALUES
(1, 5, 'Diferencia entre INNER JOIN y LEFT JOIN',
 'Hola a todos. Estoy estudiando para el parcial y no tengo claro cuando usar INNER JOIN y cuando LEFT JOIN. Entiendo que el INNER solo devuelve los que coinciden en ambas tablas pero no se exactamente cuando conviene usar uno u otro. Alguien me puede dar un ejemplo practico?',
 DATEADD(day, -12, GETDATE())),   -- ThreadId 6

(1, 8, 'Ejercicio de normalizacion - llego a 3FN pero no se si esta bien',
 'Buenas, estoy haciendo el ejercicio de normalizacion que dejo el profe. Pase de la tabla original a 1FN quitando los grupos repetitivos, luego a 2FN eliminando dependencias parciales, y en 3FN elimine las transitivas. Me quedo con 5 tablas pero no estoy seguro si hice bien el paso de 2FN a 3FN.',
 DATEADD(day, -10, GETDATE())),   -- ThreadId 7

(1, 6, 'Cuando usar un indice en SQL Server',
 'El profe menciono que los indices aceleran las consultas pero que no hay que abusar de ellos. No entendi bien por que abusar de indices puede ser malo si solo hacen las queries mas rapidas. Alguien sabe?',
 DATEADD(day, -7, GETDATE()));    -- ThreadId 8

-- ============================================================
-- CATEGORIA 2: Programacion
-- ============================================================
INSERT INTO Threads (CategoryId, UserId, Title, Message, CreatedAt) VALUES
(2, 7, 'NullPointerException en Java - no encuentro el error',
 'Me aparece un NullPointerException en la linea 47 de mi clase Estudiante.java y no entiendo por que. El objeto lo declaro arriba pero cuando llamo al metodo getNombre() falla. Tengo esto: private Estudiante est; ... est.getNombre(). Que estoy haciendo mal?',
 DATEADD(day, -11, GETDATE())),   -- ThreadId 9

(2, 9, 'Como implementar un arbol binario de busqueda en Java',
 'Estamos viendo arboles en clase y tengo que entregar la insercion y la busqueda. Tengo el nodo con valor, izquierdo y derecho. Pero no estoy segura si el metodo de insercion recursivo esta bien planteado.',
 DATEADD(day, -9, GETDATE())),    -- ThreadId 10

(2, 12, 'Git - como revertir un commit que ya subi al repositorio',
 'Se me fue un commit con errores al repo del proyecto grupal y mis companeros ya jalaron los cambios. Como hago para revertirlo sin borrar el historial? Vi algo de git revert pero no se si es lo correcto.',
 DATEADD(day, -5, GETDATE()));    -- ThreadId 11

-- ============================================================
-- CATEGORIA 3: Ingenieria de Software
-- ============================================================
INSERT INTO Threads (CategoryId, UserId, Title, Message, CreatedAt) VALUES
(3, 4, 'Diferencia entre diagrama de clases y diagrama de objetos',
 'En el parcial anterior me bajaron puntos porque confundi el diagrama de clases con el de objetos. Entiendo que el de clases es mas abstracto pero no veo bien la diferencia en la practica. Pueden explicarme con un ejemplo?',
 DATEADD(day, -14, GETDATE())),   -- ThreadId 12

(3, 10, 'Como aplicar SRP en un proyecto real',
 'Tenemos una clase que maneja la logica del usuario y tambien envia correos. El principio de responsabilidad unica dice que deberiamos separarlas? Me parece que si pero quiero confirmarlo antes de refactorizar.',
 DATEADD(day, -8, GETDATE())),    -- ThreadId 13

(3, 13, 'Patron Observer - para que sirve exactamente',
 'Estoy leyendo sobre patrones de diseno y no entiendo bien el Observer. Se que tiene un sujeto y observadores pero no veo cuando se usa en la vida real. Alguien tiene un ejemplo concreto de un sistema que lo use?',
 DATEADD(day, -3, GETDATE()));    -- ThreadId 14

-- ============================================================
-- CATEGORIA 4: Redes
-- ============================================================
INSERT INTO Threads (CategoryId, UserId, Title, Message, CreatedAt) VALUES
(4, 11, 'Subnetting con mascara /26 - me da 62 hosts, esta bien?',
 'No me sale el ejercicio de subnetting. Tenemos la red 192.168.1.0 con mascara /26 y hay que decir cuantos host validos hay. Hice el calculo: 2^6 = 64, menos 2 queda 62. Esta correcto ese razonamiento?',
 DATEADD(day, -13, GETDATE())),   -- ThreadId 15

(4, 6, 'Diferencia entre TCP y UDP y cuando usar cada uno',
 'Entiendo que TCP es orientado a conexion y UDP no. Pero en clase dijeron que YouTube usa UDP. No entenderia eso si los videos se pueden cortar. Por que una plataforma tan grande usa un protocolo sin garantia de entrega?',
 DATEADD(day, -6, GETDATE()));    -- ThreadId 16

-- ============================================================
-- CATEGORIA 5: Sistemas Operativos
-- ============================================================
INSERT INTO Threads (CategoryId, UserId, Title, Message, CreatedAt) VALUES
(5, 8, 'Deadlock - diferencia entre prevencion y deteccion',
 'Estoy estudiando para el final y el tema de deadlock me cuesta. Entiendo que ocurre cuando dos procesos se esperan mutuamente, pero como hace el SO para detectarlo? Y cual es la diferencia entre prevencion y deteccion?',
 DATEADD(day, -15, GETDATE())),   -- ThreadId 17

(5, 5, 'Diferencia entre proceso e hilo en sistemas operativos',
 'Tenemos que exponer la diferencia entre proceso e hilo. Lo que entiendo es que los hilos comparten memoria del proceso padre. Pero a nivel practico, que ventaja tiene usar hilos en vez de procesos separados?',
 DATEADD(day, -4, GETDATE()));    -- ThreadId 18

GO

-- ============================================================
-- RESPUESTAS - Bases de Datos
-- ============================================================

-- ThreadId 6: INNER JOIN vs LEFT JOIN
INSERT INTO Replies (ThreadId, UserId, Message, CreatedAt) VALUES
(6, 15, 'INNER JOIN devuelve solo las filas con coincidencia en ambas tablas. LEFT JOIN devuelve todas las filas de la tabla izquierda aunque no haya coincidencia, y en ese caso los campos de la derecha quedan NULL. Ejemplo: si tenes Estudiantes y Notas, con INNER solo ves los que tienen nota registrada. Con LEFT ves todos los estudiantes, y los que no entregaron aparecen con NULL en la columna de nota.',
 DATEADD(day, -12, DATEADD(hour, 2, GETDATE()))),
(6, 5,  'Gracias profe! Entonces si quiero ver todos los estudiantes aunque no hayan entregado uso LEFT JOIN?',
 DATEADD(day, -12, DATEADD(hour, 3, GETDATE()))),
(6, 15, 'Exacto Ana Maria. Y si solo queres los que si entregaron, INNER JOIN. Eso cae en el parcial.',
 DATEADD(day, -12, DATEADD(hour, 4, GETDATE()))),
(6, 9,  'Yo tenia la misma duda, quedo muy clara la explicacion.',
 DATEADD(day, -11, DATEADD(hour, 1, GETDATE()))),
(6, 7,  'Una duda mas: si uso RIGHT JOIN en vez de LEFT JOIN es lo mismo pero al reves?',
 DATEADD(day, -11, DATEADD(hour, 2, GETDATE()))),
(6, 15, 'Si Daniela, RIGHT JOIN devuelve todas las filas de la tabla derecha. En la practica casi siempre se usa LEFT porque es mas intuitivo reescribir el query.',
 DATEADD(day, -10, DATEADD(hour, 1, GETDATE())));

-- ThreadId 7: Normalizacion
INSERT INTO Replies (ThreadId, UserId, Message, CreatedAt) VALUES
(7, 3,  'Eduardo, para verificar 3FN fijate que cada atributo no clave dependa unicamente de la clave primaria y no de otro atributo no clave. Si hay esa dependencia transitiva hay que separarla en otra tabla. Subi el diagrama y lo revisamos.',
 DATEADD(day, -9, DATEADD(hour, 2, GETDATE()))),
(7, 8,  'Gracias profe! Lo subo manana cuando lo termine de pasar en limpio.',
 DATEADD(day, -9, DATEADD(hour, 3, GETDATE()))),
(7, 5,  'Yo tuve el mismo problema con 3FN. Lo que me ayudo fue revisar si podia deducir un atributo a partir de otro que no era la PK. Si podia, era dependencia transitiva.',
 DATEADD(day, -8, DATEADD(hour, 1, GETDATE()))),
(7, 12, 'Misma duda que tenia, gracias por la explicacion de Ana Maria tambien.',
 DATEADD(day, -7, DATEADD(hour, 1, GETDATE())));

-- ThreadId 8: Indices SQL Server
INSERT INTO Replies (ThreadId, UserId, Message, CreatedAt) VALUES
(8, 15, 'Los indices aceleran las lecturas pero ralentizan las escrituras porque cada INSERT o UPDATE tiene que actualizar tambien el indice. Ademas consumen espacio. Lo ideal es indexar columnas que usas frecuentemente en WHERE o JOIN.',
 DATEADD(day, -6, DATEADD(hour, 2, GETDATE()))),
(8, 6,  'Entonces si tengo una tabla de logs donde solo inserto datos, mejor sin indices?',
 DATEADD(day, -6, DATEADD(hour, 3, GETDATE()))),
(8, 15, 'Exactamente Carlos. Si es una tabla de insercion masiva sin muchas consultas, los indices solo te van a frenar.',
 DATEADD(day, -5, DATEADD(hour, 1, GETDATE()))),
(8, 9,  'Nunca lo habia pensado desde ese angulo. Muy util la explicacion.',
 DATEADD(day, -5, DATEADD(hour, 2, GETDATE())));

-- ============================================================
-- RESPUESTAS - Programacion
-- ============================================================

-- ThreadId 9: NullPointerException
INSERT INTO Replies (ThreadId, UserId, Message, CreatedAt) VALUES
(9, 3,  'El problema es que declaras la variable pero nunca la instancias. Con "private Estudiante est;" solo creas una referencia nula. Tenes que hacer "est = new Estudiante();" antes de llamar a sus metodos. Es el error mas comun al inicio en Java.',
 DATEADD(day, -10, DATEADD(hour, 2, GETDATE()))),
(9, 7,  'Ah entiendo! Venia de Python donde no hay que instanciar explicitamente. Ya lo corregi, funciono. Gracias profe!',
 DATEADD(day, -10, DATEADD(hour, 3, GETDATE()))),
(9, 12, 'A mi me pasaba lo mismo al inicio. Una buena practica es inicializar en el constructor para no olvidarse.',
 DATEADD(day, -10, DATEADD(hour, 4, GETDATE()))),
(9, 5,  'O usar Optional si el objeto no siempre va a existir, pero eso es mas avanzado.',
 DATEADD(day, -9, DATEADD(hour, 1, GETDATE())));

-- ThreadId 10: Arbol binario
INSERT INTO Replies (ThreadId, UserId, Message, CreatedAt) VALUES
(10, 15, 'La insercion recursiva funciona asi: si el valor es menor que el nodo actual vas a la izquierda, si es mayor a la derecha. Cuando llegas a null ahi insertas el nuevo nodo. El caso base es cuando el subArbol recibido es null.',
 DATEADD(day, -8, DATEADD(hour, 2, GETDATE()))),
(10, 9,  'Entonces el metodo recibe el nodo raiz y el valor, y si la raiz es null retorna un nodo nuevo?',
 DATEADD(day, -8, DATEADD(hour, 3, GETDATE()))),
(10, 15, 'Exacto Fernanda. Y si no es null, comparas y llamas recursivamente al hijo izquierdo o derecho.',
 DATEADD(day, -7, DATEADD(hour, 1, GETDATE()))),
(10, 6,  'Muy buena explicacion, yo tambien andaba trancado en eso.',
 DATEADD(day, -7, DATEADD(hour, 2, GETDATE())));

-- ThreadId 11: Git revert
INSERT INTO Replies (ThreadId, UserId, Message, CreatedAt) VALUES
(11, 3,  '"git revert <hash>" es lo correcto cuando ya subiste el commit. Crea un nuevo commit que deshace los cambios sin borrar el historial. Tus companeros solo hacen git pull y ven el revert aplicado.',
 DATEADD(day, -4, DATEADD(hour, 2, GETDATE()))),
(11, 12, 'Funciono perfecto! No sabia que revert crea un commit nuevo en vez de borrar el anterior. Tiene mas sentido para trabajo en equipo.',
 DATEADD(day, -4, DATEADD(hour, 3, GETDATE()))),
(11, 7,  'Yo antes usaba git reset --hard y perdia trabajo de todos jaja. Mejor revert.',
 DATEADD(day, -3, DATEADD(hour, 1, GETDATE()))),
(11, 5,  'Si, reset --hard en repos compartidos es peligroso. Revert es siempre la opcion segura.',
 DATEADD(day, -3, DATEADD(hour, 2, GETDATE())));

-- ============================================================
-- RESPUESTAS - Ingenieria de Software
-- ============================================================

-- ThreadId 12: Diagrama clases vs objetos
INSERT INTO Replies (ThreadId, UserId, Message, CreatedAt) VALUES
(12, 15, 'El diagrama de clases muestra la estructura general: que clases existen, sus atributos, metodos y relaciones. El de objetos es una foto en un momento especifico del programa en ejecucion: muestra instancias concretas con valores reales. Ejemplo: en clases tenes "Estudiante" con atributo "nombre". En objetos tenes "est1:Estudiante" con nombre="Jorge".',
 DATEADD(day, -13, DATEADD(hour, 2, GETDATE()))),
(12, 4,  'Ahhh, o sea el de objetos es como un ejemplo concreto de ejecucion del de clases?',
 DATEADD(day, -13, DATEADD(hour, 3, GETDATE()))),
(12, 15, 'Exactamente Jorge. Una muy buena forma de pensarlo.',
 DATEADD(day, -12, DATEADD(hour, 1, GETDATE()))),
(12, 11, 'Con esa explicacion me quedo clarismo. Gracias profe.',
 DATEADD(day, -12, DATEADD(hour, 2, GETDATE()))),
(12, 9,  'Yo tambien tenia esa confusion, ya me quedo claro.',
 DATEADD(day, -11, DATEADD(hour, 1, GETDATE())));

-- ThreadId 13: SRP
INSERT INTO Replies (ThreadId, UserId, Message, CreatedAt) VALUES
(13, 3,  'Si Gabriel, separa las clases. La de usuario maneja la logica del negocio relacionada al usuario. El envio de correos es una responsabilidad de infraestructura. Crea una clase EmailService aparte y la llamas desde donde la necesites.',
 DATEADD(day, -7, DATEADD(hour, 2, GETDATE()))),
(13, 10, 'Tiene sentido. Si manana cambia el servidor de correo, no deberia tener que tocar la clase de usuario.',
 DATEADD(day, -7, DATEADD(hour, 3, GETDATE()))),
(13, 3,  'Exacto! Eso es la razon para cambiar de la que habla SRP. Si dos cosas distintas pueden hacer que una clase cambie, hay que separarlas.',
 DATEADD(day, -6, DATEADD(hour, 1, GETDATE()))),
(13, 7,  'Voy a aplicar eso en mi proyecto tambien, gracias.',
 DATEADD(day, -6, DATEADD(hour, 2, GETDATE())));

-- ThreadId 14: Observer
INSERT INTO Replies (ThreadId, UserId, Message, CreatedAt) VALUES
(14, 15, 'Observer sirve cuando un objeto necesita notificar a otros automaticamente al cambiar su estado, sin conocer los detalles de esos otros objetos. Ejemplo clasico: en un sistema de noticias, cuando se publica un articulo todos los suscriptores reciben la notificacion. El articulo es el sujeto, los suscriptores son los observadores.',
 DATEADD(day, -2, DATEADD(hour, 2, GETDATE()))),
(14, 13, 'Ah, como las notificaciones de este foro! Cuando alguien responde un hilo, el autor del hilo recibe un aviso.',
 DATEADD(day, -2, DATEADD(hour, 3, GETDATE()))),
(14, 15, 'Exactamente Juliana, ese es un ejemplo perfecto.',
 DATEADD(day, -2, DATEADD(hour, 4, GETDATE()))),
(14, 4,  'Ahora si me quedo claro para que sirve. Gracias profe.',
 DATEADD(day, -1, DATEADD(hour, 1, GETDATE())));

-- ============================================================
-- RESPUESTAS - Redes
-- ============================================================

-- ThreadId 15: Subnetting /26
INSERT INTO Replies (ThreadId, UserId, Message, CreatedAt) VALUES
(15, 15, 'Correcto Hilda, 62 hosts validos. Con /26 tenes 6 bits para hosts, que da 2^6 = 64 direcciones. Restas la de red y la de broadcast y quedan 62 utilizables.',
 DATEADD(day, -12, DATEADD(hour, 2, GETDATE()))),
(15, 11, 'Que alivio! Me confundia si restar 1 o 2.',
 DATEADD(day, -12, DATEADD(hour, 3, GETDATE()))),
(15, 6,  'Siempre son 2: la primera (red) y la ultima (broadcast). Con /26 las subredes de 192.168.1.0 serian .0, .64, .128 y .192.',
 DATEADD(day, -11, DATEADD(hour, 1, GETDATE()))),
(15, 15, 'Correcto Carlos, bien calculado.',
 DATEADD(day, -11, DATEADD(hour, 2, GETDATE()))),
(15, 8,  'Guarde este hilo para el parcial jaja.',
 DATEADD(day, -10, DATEADD(hour, 1, GETDATE())));

-- ThreadId 16: TCP vs UDP
INSERT INTO Replies (ThreadId, UserId, Message, CreatedAt) VALUES
(16, 3,  'UDP es mas rapido porque no tiene el proceso de establecer conexion ni garantia de entrega. Para video en tiempo real, perder algun paquete es aceptable porque el ojo humano no lo nota. Si usaran TCP, el video se pausaria esperando reenviar cada paquete perdido, lo cual seria mucho peor.',
 DATEADD(day, -5, DATEADD(hour, 2, GETDATE()))),
(16, 6,  'Entonces es preferible perder un frame que frenar todo el stream?',
 DATEADD(day, -5, DATEADD(hour, 3, GETDATE()))),
(16, 3,  'Exactamente Carlos. Por eso las videollamadas y el streaming usan UDP o protocolos sobre UDP como RTP.',
 DATEADD(day, -4, DATEADD(hour, 1, GETDATE()))),
(16, 5,  'Y para descargar un archivo si se usa TCP porque no podemos perder datos.',
 DATEADD(day, -4, DATEADD(hour, 2, GETDATE()))),
(16, 3,  'Correcto Ana Maria. Esa es la distincion clave para el examen.',
 DATEADD(day, -3, DATEADD(hour, 1, GETDATE())));

-- ============================================================
-- RESPUESTAS - Sistemas Operativos
-- ============================================================

-- ThreadId 17: Deadlock
INSERT INTO Replies (ThreadId, UserId, Message, CreatedAt) VALUES
(17, 15, 'Prevencion: el sistema se disena para que una de las cuatro condiciones del deadlock nunca se cumpla, por ejemplo obligando a pedir todos los recursos de una vez. Deteccion: se permite que el deadlock ocurra pero el SO lo detecta con algoritmos como el grafo de asignacion de recursos, y luego mata procesos para resolverlo.',
 DATEADD(day, -14, DATEADD(hour, 2, GETDATE()))),
(17, 8,  'Entonces prevencion es mas costosa pero segura, y deteccion es mas flexible pero requiere recuperacion posterior?',
 DATEADD(day, -14, DATEADD(hour, 3, GETDATE()))),
(17, 15, 'Muy bien resumido Eduardo, exactamente asi.',
 DATEADD(day, -13, DATEADD(hour, 1, GETDATE()))),
(17, 4,  'Y el algoritmo del banquero entra en prevencion o deteccion?',
 DATEADD(day, -13, DATEADD(hour, 2, GETDATE()))),
(17, 15, 'En prevencion Jorge. Antes de asignar un recurso, simula si el estado resultante es seguro. Si no lo es, el proceso espera.',
 DATEADD(day, -12, DATEADD(hour, 1, GETDATE()))),
(17, 11, 'Esto me aclaro mucho el tema para el final. Gracias.',
 DATEADD(day, -11, DATEADD(hour, 1, GETDATE())));

-- ThreadId 18: Proceso vs hilo
INSERT INTO Replies (ThreadId, UserId, Message, CreatedAt) VALUES
(18, 3,  'Los hilos comparten el espacio de memoria del proceso, entonces comunicarse entre ellos es mas rapido. Ademas crear un hilo es mas liviano que crear un proceso nuevo. La desventaja es que si un hilo falla gravemente puede afectar a todos los demas del proceso.',
 DATEADD(day, -3, DATEADD(hour, 2, GETDATE()))),
(18, 5,  'Entonces para un servidor que maneja muchas peticiones simultaneas conviene hilos?',
 DATEADD(day, -3, DATEADD(hour, 3, GETDATE()))),
(18, 3,  'Exacto. Apache y Nginx hacen exactamente eso: cada peticion la maneja un hilo del mismo proceso servidor.',
 DATEADD(day, -2, DATEADD(hour, 1, GETDATE()))),
(18, 6,  'En Java podemos usar Thread o Runnable para esto?',
 DATEADD(day, -2, DATEADD(hour, 2, GETDATE()))),
(18, 3,  'Si Carlos. En Java moderno tambien tenes ExecutorService que maneja un pool de hilos de forma mas eficiente que crear threads manualmente.',
 DATEADD(day, -1, DATEADD(hour, 1, GETDATE()))),
(18, 9,  'Justo andaba buscando como manejar concurrencia para el proyecto final. Gracias profe.',
 DATEADD(day, -1, DATEADD(hour, 2, GETDATE())));
