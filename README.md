[README.txt](https://github.com/user-attachments/files/34804088/README.txt)
# Don Juan Market

**Proyecto final — Procesos y Algoritmos II**  
**Lenguaje:** C# (.NET)  
**Tipo de aplicación:** Consola

## Descripción

Don Juan Market es un programa de consola que permite administrar los productos de un supermercado y organizar la atención de los clientes. El proyecto utiliza dos estructuras de datos estudiadas en el curso: un **árbol AVL** para manejar el inventario y una **cola de prioridad implementada con un Min Heap** para atender a los clientes.

## Objetivo

Aplicar árboles AVL y colas de prioridad en un sistema sencillo que permita registrar, consultar, modificar y eliminar productos, además de administrar una fila de atención de clientes.

## Funciones del programa

### Inventario (árbol AVL)

1. Insertar un producto con código, nombre, categoría, precio y cantidad disponible.
2. Buscar productos mediante su código.
3. Actualizar nombre, categoría, precio y stock de un producto existente.
4. Eliminar productos por código.
5. Mostrar el inventario ordenado por código mediante recorrido **In-Orden**.

El AVL mantiene su balance mediante rotaciones a la izquierda y a la derecha cuando son necesarias.

### Atención de clientes (Min Heap)

6. Registrar clientes con nombre, tipo, cantidad de productos y fecha/hora de llegada.
7. Atender al siguiente cliente según su prioridad.
8. Mostrar los clientes que están esperando.

El orden de prioridad es:

- **1. VIP** — prioridad más alta.
- **2. Adulto mayor** — prioridad intermedia.
- **3. Regular** — prioridad normal.

Si dos clientes tienen la misma prioridad, se respeta el orden de llegada (**FIFO**). El Heap se reorganiza después de agregar o atender a un cliente.

### Otras opciones

9. Mostrar estadísticas del inventario y de los clientes en espera.
10. Salir del programa.

## Requisitos

- Computadora con Windows, Linux o macOS.
- SDK de **.NET 8** instalado (o el SDK compatible con el archivo `.csproj` utilizado).
- Visual Studio Code, Visual Studio o una terminal.

## Cómo ejecutar el proyecto

1. Descarga o clona este repositorio.
2. Abre la carpeta **`ConsoleApp1`** en Visual Studio Code. Debe ser la carpeta que contiene el archivo `.csproj`.
3. Abre una terminal en esa misma carpeta.
4. Ejecuta:

```bash
dotnet run
```

Si la terminal se abre en la carpeta raíz del repositorio, primero entra en la carpeta del proyecto:

```bash
cd ConsoleApp1
dotnet run
```

**Nota:** Si aparece el mensaje «No se ha podido encontrar un proyecto para ejecutar», verifica que la terminal esté situada en la carpeta donde se encuentra el archivo `.csproj`.

## Almacenamiento de la información

El programa utiliza archivos de texto para guardar y recuperar la información del inventario y de los clientes pendientes:

- `inventario.txt`
- `clientes.txt`

Estos archivos se generan o leen en el directorio de trabajo desde donde se ejecuta el programa. No es necesario crearlos manualmente antes del primer uso.

## Estructuras de datos utilizadas

**Árbol AVL:** es un árbol binario de búsqueda que se mantiene balanceado. Permite insertar, buscar y eliminar productos por su código de manera eficiente.

**Min Heap:** es una estructura que mantiene al cliente de mayor prioridad en la raíz. Las operaciones de inserción y extracción reorganizan los elementos con los procedimientos conocidos como *subir* y *bajar*.

## Validaciones

El sistema controla, entre otros casos, los códigos de productos repetidos, los campos vacíos, los precios y las cantidades inválidas, las opciones de menú incorrectas y la atención cuando no hay clientes en espera.

## Documentación

Como parte de la entrega académica se prepara un manual de usuario con capturas de pantalla y un diagrama UML de clases. Estos documentos pueden adjuntarse por separado o incluirse en la documentación PDF solicitada por el docente.

## Repositorio

https://github.com/abrahambarrondo2112-jpg/Proyecto-Final-

## Propósito académico

Este programa se desarrolló como proyecto final del curso **Procesos y Algoritmos II**, para practicar estructuras de datos y programación orientada a objetos en C#.
