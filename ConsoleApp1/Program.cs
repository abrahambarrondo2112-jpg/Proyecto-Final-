using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

// Aqui empieza el programa de Don Juan Market.
// ========================================
// INICIO DEL PROGRAMA
// ========================================
class Program
{
    static void Main(string[] args)
    {
        Supermercado supermercado = new Supermercado();
        Menu menu = new Menu(supermercado);
        menu.Ejecutar();
    }
}

// ========================================
// CLASE PRODUCTO: guarda los datos de cada producto
// ========================================

class Producto
{
    public string codigo { get; private set; }
    public string nombre { get; set; }
    public string categoria { get; set; }
    public double precio { get; set; }
    public int stock { get; set; }

    public Producto(string codigo, string nombre,
                    string categoria, double precio, int stock)
    {
        this.codigo = codigo;
        this.nombre = nombre;
        this.categoria = categoria;
        this.precio = precio;
        this.stock = stock;
    }
}

// ========================================
// NODO DEL ARBOL AVL: guarda un producto y sus hijos
// ========================================

class NodoAVL
{
    public Producto producto { get; set; }
    public NodoAVL izquierdo { get; set; }
    public NodoAVL derecho { get; set; }
    public int altura { get; set; }

    public NodoAVL(Producto producto)
    {
        this.producto = producto;
        izquierdo = null;
        derecho = null;
        altura = 1;
    }
}

// ========================================
// ARBOL AVL DEL INVENTARIO: organiza los productos por codigo
// ========================================

class ArbolAVL
{
    public NodoAVL raiz { get; private set; }

    public void Insertar(Producto producto)
    {
        raiz = InsertarRecursivo(raiz, producto);
    }

    private NodoAVL InsertarRecursivo(NodoAVL nodo, Producto producto)
    {
        if (nodo == null)
        {
            return new NodoAVL(producto);
        }

        int comparacion = string.Compare(
            producto.codigo,
            nodo.producto.codigo,
            StringComparison.OrdinalIgnoreCase);

        if (comparacion < 0)
        {
            nodo.izquierdo = InsertarRecursivo(
                nodo.izquierdo, producto);
        }
        else if (comparacion > 0)
        {
            nodo.derecho = InsertarRecursivo(
                nodo.derecho, producto);
        }
        else
        {
            return nodo;
        }

        nodo.altura = 1 + Math.Max(
            ObtenerAltura(nodo.izquierdo),
            ObtenerAltura(nodo.derecho));

        int balance = ObtenerBalance(nodo);

        // Caso izquierda-izquierda
        if (balance > 1 &&
            string.Compare(
                producto.codigo,
                nodo.izquierdo.producto.codigo,
                StringComparison.OrdinalIgnoreCase) < 0)
        {
            return RotacionDerecha(nodo);
        }

        // Caso derecha-derecha
        if (balance < -1 &&
            string.Compare(
                producto.codigo,
                nodo.derecho.producto.codigo,
                StringComparison.OrdinalIgnoreCase) > 0)
        {
            return RotacionIzquierda(nodo);
        }

        // Caso izquierda-derecha
        if (balance > 1 &&
            string.Compare(
                producto.codigo,
                nodo.izquierdo.producto.codigo,
                StringComparison.OrdinalIgnoreCase) > 0)
        {
            nodo.izquierdo = RotacionIzquierda(
                nodo.izquierdo);

            return RotacionDerecha(nodo);
        }

        // Caso derecha-izquierda
        if (balance < -1 &&
            string.Compare(
                producto.codigo,
                nodo.derecho.producto.codigo,
                StringComparison.OrdinalIgnoreCase) < 0)
        {
            nodo.derecho = RotacionDerecha(
                nodo.derecho);

            return RotacionIzquierda(nodo);
        }

        return nodo;
    }

    private int ObtenerAltura(NodoAVL nodo)
    {
        if (nodo == null)
        {
            return 0;
        }

        return nodo.altura;
    }

    private int ObtenerBalance(NodoAVL nodo)
    {
        if (nodo == null)
        {
            return 0;
        }

        return ObtenerAltura(nodo.izquierdo) -
               ObtenerAltura(nodo.derecho);
    }

    // Rotacion simple hacia la derecha
    private NodoAVL RotacionDerecha(NodoAVL y)
    {
        NodoAVL x = y.izquierdo;
        NodoAVL temporal = x.derecho;

        x.derecho = y;
        y.izquierdo = temporal;

        y.altura = 1 + Math.Max(
            ObtenerAltura(y.izquierdo),
            ObtenerAltura(y.derecho));

        x.altura = 1 + Math.Max(
            ObtenerAltura(x.izquierdo),
            ObtenerAltura(x.derecho));

        return x;
    }

    // Rotacion simple hacia la izquierda
    private NodoAVL RotacionIzquierda(NodoAVL x)
    {
        NodoAVL y = x.derecho;
        NodoAVL temporal = y.izquierdo;

        y.izquierdo = x;
        x.derecho = temporal;

        x.altura = 1 + Math.Max(
            ObtenerAltura(x.izquierdo),
            ObtenerAltura(x.derecho));

        y.altura = 1 + Math.Max(
            ObtenerAltura(y.izquierdo),
            ObtenerAltura(y.derecho));

        return y;
    }

    // ========================================
    // BUSCAR PRODUCTO
    // ========================================

    public Producto Buscar(string codigo)
    {
        return BuscarRecursivo(raiz, codigo);
    }

    private Producto BuscarRecursivo(NodoAVL nodo, string codigo)
    {
        if (nodo == null)
        {
            return null;
        }

        int comparacion = string.Compare(
            codigo,
            nodo.producto.codigo,
            StringComparison.OrdinalIgnoreCase);

        if (comparacion == 0)
        {
            return nodo.producto;
        }
        else if (comparacion < 0)
        {
            return BuscarRecursivo(nodo.izquierdo, codigo);
        }
        else
        {
            return BuscarRecursivo(nodo.derecho, codigo);
        }
    }

    // ========================================
    // ELIMINAR PRODUCTO
    // ========================================

    public bool Eliminar(string codigo)
    {
        if (Buscar(codigo) == null)
        {
            return false;
        }

        raiz = EliminarRecursivo(raiz, codigo);
        return true;
    }

    private NodoAVL EliminarRecursivo(
        NodoAVL nodo, string codigo)
    {
        if (nodo == null)
        {
            return null;
        }

        int comparacion = string.Compare(
            codigo,
            nodo.producto.codigo,
            StringComparison.OrdinalIgnoreCase);

        if (comparacion < 0)
        {
            nodo.izquierdo = EliminarRecursivo(
                nodo.izquierdo, codigo);
        }
        else if (comparacion > 0)
        {
            nodo.derecho = EliminarRecursivo(
                nodo.derecho, codigo);
        }
        else
        {
            // Nodo sin hijos
            if (nodo.izquierdo == null &&
                nodo.derecho == null)
            {
                return null;
            }

            // Nodo con un hijo
            if (nodo.izquierdo == null)
            {
                return nodo.derecho;
            }

            if (nodo.derecho == null)
            {
                return nodo.izquierdo;
            }

            // Nodo con dos hijos
            NodoAVL sucesor = ObtenerMenor(
                nodo.derecho);

            nodo.producto = sucesor.producto;

            nodo.derecho = EliminarRecursivo(
                nodo.derecho,
                sucesor.producto.codigo);
        }

        // Actualizar altura del nodo
        nodo.altura = 1 + Math.Max(
            ObtenerAltura(nodo.izquierdo),
            ObtenerAltura(nodo.derecho));

        int balance = ObtenerBalance(nodo);

        // Rebalancear despues de eliminar
        if (balance > 1 &&
            ObtenerBalance(nodo.izquierdo) >= 0)
        {
            return RotacionDerecha(nodo);
        }

        if (balance > 1 &&
            ObtenerBalance(nodo.izquierdo) < 0)
        {
            nodo.izquierdo = RotacionIzquierda(
                nodo.izquierdo);

            return RotacionDerecha(nodo);
        }

        if (balance < -1 &&
            ObtenerBalance(nodo.derecho) <= 0)
        {
            return RotacionIzquierda(nodo);
        }

        if (balance < -1 &&
            ObtenerBalance(nodo.derecho) > 0)
        {
            nodo.derecho = RotacionDerecha(
                nodo.derecho);

            return RotacionIzquierda(nodo);
        }

        return nodo;
    }

    private NodoAVL ObtenerMenor(NodoAVL nodo)
    {
        NodoAVL actual = nodo;

        while (actual.izquierdo != null)
        {
            actual = actual.izquierdo;
        }

        return actual;
    }

    // ========================================
    // MOSTRAR INVENTARIO EN ORDEN
    // ========================================

    public void MostrarInventario()
    {
        if (raiz == null)
        {
            Console.WriteLine(
                "El inventario está vacío.");
        }
        else
        {
            MostrarInOrden(raiz);
        }
    }

    private void MostrarInOrden(NodoAVL nodo)
    {
        if (nodo != null)
        {
            MostrarInOrden(nodo.izquierdo);

            Console.WriteLine("--------------------------------");
            Console.WriteLine(
                "Código: " + nodo.producto.codigo);
            Console.WriteLine(
                "Nombre: " + nodo.producto.nombre);
            Console.WriteLine(
                "Categoría: " + nodo.producto.categoria);
            Console.WriteLine(
                "Precio: Q" +
                nodo.producto.precio.ToString("0.00"));
            Console.WriteLine(
                "Stock: " + nodo.producto.stock);

            MostrarInOrden(nodo.derecho);
        }
    }

    // ========================================
    // GUARDAR INVENTARIO
    // ========================================

    public void GuardarArchivo(string ruta)
    {
        using (StreamWriter archivo =
            new StreamWriter(ruta))
        {
            GuardarRecursivo(raiz, archivo);
        }
    }

    private void GuardarRecursivo(
        NodoAVL nodo, StreamWriter archivo)
    {
        if (nodo != null)
        {
            GuardarRecursivo(nodo.izquierdo, archivo);

            archivo.WriteLine(
                nodo.producto.codigo + "|" +
                nodo.producto.nombre + "|" +
                nodo.producto.categoria + "|" +
                nodo.producto.precio.ToString(
                    CultureInfo.InvariantCulture) + "|" +
                nodo.producto.stock);

            GuardarRecursivo(nodo.derecho, archivo);
        }
    }
}

// ========================================
// CLASE CLIENTE: informacion de cada persona en espera
// ========================================

class Cliente
{
    public string nombre { get; set; }
    public string tipo { get; private set; }
    public int prioridad { get; private set; }
    public long ordenLlegada { get; private set; }

    // Datos de la compra y hora de llegada
    public int cantidadProductos { get; private set; }
    public DateTime fechaLlegada { get; private set; }

    public Cliente(
        string nombre,
        string tipo,
        int prioridad,
        long ordenLlegada,
        int cantidadProductos,
        DateTime fechaLlegada)
    {
        this.nombre = nombre;
        this.tipo = tipo;
        this.prioridad = prioridad;
        this.ordenLlegada = ordenLlegada;
        this.cantidadProductos = cantidadProductos;
        this.fechaLlegada = fechaLlegada;
    }
}

// ========================================
// COLA DE PRIORIDAD - MIN HEAP: atiende primero al mas prioritario
// ========================================

class ColaPrioridad
{
    private List<Cliente> heap =
        new List<Cliente>();

    private long siguienteOrden = 1;

    public int Cantidad
    {
        get { return heap.Count; }
    }

    // ========================================
    // AGREGAR CLIENTE
    // ========================================

    public void Agregar(
        string nombre,
        string tipo,
        int prioridad,
        int cantidadProductos)
    {
        // Se registra la fecha y hora automáticamente
        DateTime fechaActual = DateTime.Now;

        Cliente nuevo = new Cliente(
            nombre,
            tipo,
            prioridad,
            siguienteOrden++,
            cantidadProductos,
            fechaActual);

        heap.Add(nuevo);

        Subir(heap.Count - 1);
    }

    // ========================================
    // ATENDER SIGUIENTE CLIENTE
    // ========================================

    public Cliente AtenderSiguiente()
    {
        if (heap.Count == 0)
        {
            return null;
        }

        Cliente primero = heap[0];

        Cliente ultimo = heap[heap.Count - 1];

        heap.RemoveAt(heap.Count - 1);

        if (heap.Count > 0)
        {
            heap[0] = ultimo;
            Bajar(0);
        }

        return primero;
    }

    // ========================================
    // MOSTRAR CLIENTES EN ESPERA
    // ========================================

    public void MostrarCola()
    {
        if (heap.Count == 0)
        {
            Console.WriteLine(
                "No hay clientes en espera.");
            return;
        }

        List<Cliente> ordenados =
            new List<Cliente>(heap);

        ordenados.Sort((a, b) => Comparar(a, b));

        Console.WriteLine(
            "\n--- CLIENTES EN ESPERA (ORDEN DE ATENCIÓN) ---");

        for (int i = 0; i < ordenados.Count; i++)
        {
            Cliente cliente = ordenados[i];

            string cantidadTexto =
                cliente.cantidadProductos == 0
                ? "No registrada"
                : cliente.cantidadProductos.ToString();

            string fechaTexto =
                cliente.fechaLlegada == DateTime.MinValue
                ? "No registrada"
                : cliente.fechaLlegada.ToString(
                    "dd/MM/yyyy HH:mm:ss");

            Console.WriteLine("--------------------------------");
            Console.WriteLine(
                "Turno de atención: " + (i + 1));
            Console.WriteLine(
                "Nombre: " + cliente.nombre);
            Console.WriteLine(
                "Tipo: " + cliente.tipo);
            Console.WriteLine(
                "Prioridad: " + cliente.prioridad);
            Console.WriteLine(
                "Orden de llegada: " + cliente.ordenLlegada);
            Console.WriteLine(
                "Cantidad de productos: " + cantidadTexto);
            Console.WriteLine(
                "Fecha y hora: " + fechaTexto);
        }
    }

    // ========================================
    // GUARDAR CLIENTES EN ARCHIVO
    // ========================================

    public void GuardarArchivo(string ruta)
    {
        using (StreamWriter archivo =
            new StreamWriter(ruta))
        {
            foreach (Cliente cliente in heap)
            {
                archivo.WriteLine(
                    cliente.nombre + "|" +
                    cliente.tipo + "|" +
                    cliente.prioridad + "|" +
                    cliente.ordenLlegada + "|" +
                    cliente.cantidadProductos + "|" +
                    cliente.fechaLlegada.ToString(
                        "O",
                        CultureInfo.InvariantCulture));
            }
        }
    }

    // ========================================
    // CARGAR CLIENTES DESDE ARCHIVO
    // ========================================

    public int CargarArchivo(string ruta)
    {
        if (!File.Exists(ruta))
        {
            return 0;
        }

        int cargados = 0;

        foreach (string linea in File.ReadLines(ruta))
        {
            string[] datos = linea.Split('|');

            // Se aceptan:
            // 4 campos: formato anterior
            // 6 campos: formato actualizado

            if (datos.Length != 4 &&
                datos.Length != 6)
            {
                continue;
            }

            if (string.IsNullOrWhiteSpace(datos[0]) ||
                !int.TryParse(
                    datos[2], out int prioridad) ||
                !long.TryParse(
                    datos[3], out long ordenLlegada) ||
                ordenLlegada < 1)
            {
                continue;
            }

            string nombre = datos[0];
            string tipo = datos[1];

            bool tipoValido =
                (tipo == "VIP" && prioridad == 1) ||
                (tipo == "Adulto mayor" && prioridad == 2) ||
                (tipo == "Regular" && prioridad == 3);

            if (!tipoValido)
            {
                continue;
            }

            int cantidadProductos = 0;
            DateTime fechaLlegada = DateTime.MinValue;

            // Leer los dos campos nuevos
            if (datos.Length == 6)
            {
                if (!int.TryParse(
                    datos[4], out cantidadProductos) ||
                    cantidadProductos < 0)
                {
                    continue;
                }

                if (!DateTime.TryParseExact(
                    datos[5],
                    "O",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.RoundtripKind,
                    out fechaLlegada))
                {
                    continue;
                }
            }

            Cliente cliente = new Cliente(
                nombre,
                tipo,
                prioridad,
                ordenLlegada,
                cantidadProductos,
                fechaLlegada);

            heap.Add(cliente);

            Subir(heap.Count - 1);

            // Mantener el orden de llegada
            // incluso después de cerrar el programa
            if (ordenLlegada >= siguienteOrden)
            {
                siguienteOrden = ordenLlegada + 1;
            }

            cargados++;
        }

        return cargados;
    }

    // ========================================
    // CONTAR CLIENTES POR TIPO
    // ========================================

    public int ContarTipo(string tipo)
    {
        int total = 0;

        foreach (Cliente cliente in heap)
        {
            if (cliente.tipo == tipo)
            {
                total++;
            }
        }

        return total;
    }

    // ========================================
    // CONTAR PRODUCTOS DE CLIENTES EN ESPERA
    // ========================================

    public int ContarArticulosEnEspera()
    {
        int total = 0;

        foreach (Cliente cliente in heap)
        {
            total += cliente.cantidadProductos;
        }

        return total;
    }

    // ========================================
    // COMPARAR PRIORIDAD Y LLEGADA
    // ========================================

    private int Comparar(Cliente a, Cliente b)
    {
        // Prioridad 1 = VIP
        // Prioridad 2 = Adulto mayor
        // Prioridad 3 = Regular

        if (a.prioridad != b.prioridad)
        {
            return a.prioridad.CompareTo(b.prioridad);
        }

        // Misma prioridad:
        // se atiende primero al que llegó antes
        return a.ordenLlegada.CompareTo(
            b.ordenLlegada);
    }

    // ========================================
    // REORGANIZAR HEAP HACIA ARRIBA
    // ========================================

    private void Subir(int indice)
    {
        while (indice > 0)
        {
            int padre = (indice - 1) / 2;

            if (Comparar(
                heap[indice], heap[padre]) >= 0)
            {
                break;
            }

            Intercambiar(indice, padre);

            indice = padre;
        }
    }

    // ========================================
    // REORGANIZAR HEAP HACIA ABAJO
    // ========================================

    private void Bajar(int indice)
    {
        while (true)
        {
            int izquierdo = 2 * indice + 1;
            int derecho = 2 * indice + 2;

            int mejor = indice;

            if (izquierdo < heap.Count &&
                Comparar(
                    heap[izquierdo], heap[mejor]) < 0)
            {
                mejor = izquierdo;
            }

            if (derecho < heap.Count &&
                Comparar(
                    heap[derecho], heap[mejor]) < 0)
            {
                mejor = derecho;
            }

            if (mejor == indice)
            {
                break;
            }

            Intercambiar(indice, mejor);

            indice = mejor;
        }
    }

    private void Intercambiar(int a, int b)
    {
        Cliente temporal = heap[a];

        heap[a] = heap[b];
        heap[b] = temporal;
    }
}

// ========================================
// SUPERMERCADO: JUNTA EL INVENTARIO Y LA COLA
// ========================================
class Supermercado
{
    public ArbolAVL Inventario { get; private set; }
    public ColaPrioridad Clientes { get; private set; }

    public Supermercado()
    {
        Inventario = new ArbolAVL();
        Clientes = new ColaPrioridad();
    }
}

// ========================================
// MENU: MUESTRA LAS OPCIONES AL USUARIO
// ========================================
class Menu
{
    private ArbolAVL inventario;
    private ColaPrioridad colaClientes;
    private readonly string rutaArchivo = "inventario.txt";
    private readonly string rutaClientes = "clientes.txt";

    public Menu(Supermercado supermercado)
    {
        inventario = supermercado.Inventario;
        colaClientes = supermercado.Clientes;
    }

    // ========================================
    // MOSTRAR MENU
    // ========================================

    public void Ejecutar()
    {
        CargarInventario();
        CargarClientes();

        int opcion = 0;

        do
        {
            Console.WriteLine(
                "\n================================");

            Console.WriteLine(
                "       DON JUAN MARKET");

            Console.WriteLine(
                "================================");

            Console.WriteLine(
                "--- GESTIÓN DE INVENTARIO ---");

            Console.WriteLine("1. Insertar producto");
            Console.WriteLine("2. Buscar producto");
            Console.WriteLine("3. Actualizar producto");
            Console.WriteLine("4. Eliminar producto");
            Console.WriteLine("5. Mostrar inventario ordenado");

            Console.WriteLine(
                "--- ATENCIÓN DE CLIENTES ---");

            Console.WriteLine(
                "6. Registrar cliente en la cola");

            Console.WriteLine(
                "7. Atender siguiente cliente");

            Console.WriteLine(
                "8. Mostrar clientes en espera");

            Console.WriteLine("--- ESTADÍSTICAS ---");

            Console.WriteLine(
                "9. Mostrar estadísticas del sistema");

            Console.WriteLine("10. Salir");

            Console.WriteLine(
                "================================");

            opcion = LeerEntero(
                "Seleccione una opción (1-10): ",
                1, 10);

            switch (opcion)
            {
                case 1:
                    InsertarProducto();
                    break;

                case 2:
                    BuscarProducto();
                    break;

                case 3:
                    ActualizarProducto();
                    break;

                case 4:
                    EliminarProducto();
                    break;

                case 5:
                    Console.WriteLine(
                        "\n--- INVENTARIO ORDENADO POR CÓDIGO ---");

                    inventario.MostrarInventario();
                    break;

                case 6:
                    RegistrarCliente();
                    break;

                case 7:
                    AtenderCliente();
                    break;

                case 8:
                    colaClientes.MostrarCola();
                    break;

                case 9:
                    MostrarEstadisticas();
                    break;

                case 10:
                    GuardarInventario();
                    GuardarClientes();

                    Console.WriteLine(
                        "Gracias por utilizar Don Juan Market.");
                    break;
            }

        } while (opcion != 10);
    }

    // ========================================
    // CARGAR INVENTARIO
    // ========================================

    private void CargarInventario()
    {
        if (!File.Exists(rutaArchivo))
        {
            Console.WriteLine(
                "No hay un inventario guardado. Se iniciará uno nuevo.");

            return;
        }

        try
        {
            string[] lineas =
                File.ReadAllLines(rutaArchivo);

            int cargados = 0;

            foreach (string linea in lineas)
            {
                string[] datos = linea.Split('|');

                if (datos.Length == 5 &&
                    double.TryParse(
                        datos[3],
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out double precio) &&
                    int.TryParse(
                        datos[4], out int stock) &&
                    precio > 0 &&
                    !double.IsNaN(precio) &&
                    !double.IsInfinity(precio) &&
                    stock >= 0 &&
                    !string.IsNullOrWhiteSpace(datos[0]) &&
                    !string.IsNullOrWhiteSpace(datos[1]) &&
                    !string.IsNullOrWhiteSpace(datos[2]) &&
                    inventario.Buscar(datos[0]) == null)
                {
                    Producto producto = new Producto(
                        datos[0],
                        datos[1],
                        datos[2],
                        precio,
                        stock);

                    inventario.Insertar(producto);

                    cargados++;
                }
            }

            Console.WriteLine(
                "Productos cargados: " + cargados);
        }
        catch (IOException)
        {
            Console.WriteLine(
                "No se pudo leer el archivo del inventario.");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine(
                "No hay permiso para leer el inventario.");
        }
    }

    // ========================================
    // GUARDAR INVENTARIO
    // ========================================

    private void GuardarInventario()
    {
        try
        {
            inventario.GuardarArchivo(
                rutaArchivo);
        }
        catch (IOException)
        {
            Console.WriteLine(
                "Error: no se pudo guardar el inventario.");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine(
                "No hay permiso para guardar el inventario.");
        }
    }

    // ========================================
    // CARGAR CLIENTES
    // ========================================

    private void CargarClientes()
    {
        try
        {
            int cargados =
                colaClientes.CargarArchivo(rutaClientes);

            Console.WriteLine(
                "Clientes pendientes cargados: " +
                cargados);
        }
        catch (IOException)
        {
            Console.WriteLine(
                "No se pudo leer el archivo de clientes.");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine(
                "No hay permiso para leer el archivo de clientes.");
        }
    }

    // ========================================
    // GUARDAR CLIENTES
    // ========================================

    private void GuardarClientes()
    {
        try
        {
            colaClientes.GuardarArchivo(
                rutaClientes);
        }
        catch (IOException)
        {
            Console.WriteLine(
                "Error: no se pudo guardar la cola de clientes.");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine(
                "No hay permiso para guardar los clientes.");
        }
    }

    // ========================================
    // LEER TEXTO
    // ========================================

    private string LeerTexto(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);

            string texto = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(texto))
            {
                texto = texto.Trim();

                if (texto.Contains("|"))
                {
                    Console.WriteLine(
                        "No se permite utilizar el símbolo |.");

                    continue;
                }

                return texto;
            }

            Console.WriteLine(
                "El dato no puede estar vacío.");
        }
    }

    // ========================================
    // LEER ENTERO VALIDADO
    // ========================================

    private int LeerEntero(
        string mensaje, int minimo, int maximo)
    {
        int numero;

        while (true)
        {
            Console.Write(mensaje);

            if (int.TryParse(
                Console.ReadLine(), out numero) &&
                numero >= minimo &&
                numero <= maximo)
            {
                return numero;
            }

            Console.WriteLine(
                "Ingrese un número entre " +
                minimo + " y " + maximo + ".");
        }
    }

    // ========================================
    // LEER PRECIO
    // ========================================

    private double LeerPrecio(string mensaje)
    {
        double precio;

        while (true)
        {
            Console.Write(mensaje);

            if (double.TryParse(
                Console.ReadLine(), out precio) &&
                precio > 0 &&
                !double.IsInfinity(precio) &&
                !double.IsNaN(precio))
            {
                return precio;
            }

            Console.WriteLine(
                "Ingrese un precio válido mayor que cero.");
        }
    }

    // ========================================
    // LEER STOCK
    // ========================================

    private int LeerStock(string mensaje)
    {
        int stock;

        while (true)
        {
            Console.Write(mensaje);

            if (int.TryParse(
                Console.ReadLine(), out stock) &&
                stock >= 0)
            {
                return stock;
            }

            Console.WriteLine(
                "El stock debe ser un entero mayor o igual a cero.");
        }
    }

    // ========================================
    // INSERTAR PRODUCTO
    // ========================================

    private void InsertarProducto()
    {
        Console.WriteLine(
            "\n--- INSERTAR PRODUCTO ---");

        string codigo = LeerTexto(
            "Ingrese el código: ");

        if (inventario.Buscar(codigo) != null)
        {
            Console.WriteLine(
                "Ya existe un producto con ese código.");

            return;
        }

        string nombre = LeerTexto(
            "Ingrese el nombre: ");

        string categoria = LeerTexto(
            "Ingrese la categoría: ");

        double precio = LeerPrecio(
            "Ingrese el precio: Q");

        int stock = LeerStock(
            "Ingrese la cantidad en stock: ");

        Producto nuevo = new Producto(
            codigo, nombre, categoria, precio, stock);

        inventario.Insertar(nuevo);

        GuardarInventario();

        Console.WriteLine(
            "Producto insertado correctamente.");
    }

    // ========================================
    // BUSCAR PRODUCTO
    // ========================================

    private void BuscarProducto()
    {
        Console.WriteLine(
            "\n--- BUSCAR PRODUCTO ---");

        string codigo = LeerTexto(
            "Ingrese el código del producto: ");

        Producto producto =
            inventario.Buscar(codigo);

        if (producto == null)
        {
            Console.WriteLine(
                "Producto no encontrado.");
        }
        else
        {
            MostrarDatosProducto(producto);
        }
    }

    // ========================================
    // MOSTRAR DATOS DE PRODUCTO
    // ========================================

    private void MostrarDatosProducto(
        Producto producto)
    {
        Console.WriteLine(
            "\n--- DATOS DEL PRODUCTO ---");

        Console.WriteLine(
            "Código: " + producto.codigo);

        Console.WriteLine(
            "Nombre: " + producto.nombre);

        Console.WriteLine(
            "Categoría: " + producto.categoria);

        Console.WriteLine(
            "Precio: Q" +
            producto.precio.ToString("0.00"));

        Console.WriteLine(
            "Stock: " + producto.stock);
    }

    // ========================================
    // ACTUALIZAR PRODUCTO
    // ========================================

    private void ActualizarProducto()
    {
        Console.WriteLine(
            "\n--- ACTUALIZAR PRODUCTO ---");

        string codigo = LeerTexto(
            "Ingrese el código del producto: ");

        Producto producto =
            inventario.Buscar(codigo);

        if (producto == null)
        {
            Console.WriteLine(
                "Producto no encontrado.");

            return;
        }

        MostrarDatosProducto(producto);

        Console.WriteLine(
            "\nIngrese los nuevos datos:");

        string nombre = LeerTexto(
            "Nuevo nombre: ");

        string categoria = LeerTexto(
            "Nueva categoría: ");

        double precio = LeerPrecio(
            "Nuevo precio: Q");

        int stock = LeerStock(
            "Nuevo stock: ");

        producto.nombre = nombre;
        producto.categoria = categoria;
        producto.precio = precio;
        producto.stock = stock;

        GuardarInventario();

        Console.WriteLine(
            "Producto actualizado correctamente.");
    }

    // ========================================
    // ELIMINAR PRODUCTO
    // ========================================

    private void EliminarProducto()
    {
        Console.WriteLine(
            "\n--- ELIMINAR PRODUCTO ---");

        string codigo = LeerTexto(
            "Ingrese el código que desea eliminar: ");

        Producto producto =
            inventario.Buscar(codigo);

        if (producto == null)
        {
            Console.WriteLine(
                "No existe un producto con ese código.");

            return;
        }

        MostrarDatosProducto(producto);

        string confirmacion = LeerTexto(
            "¿Está seguro de eliminarlo? Escriba SI o NO: ");

        if (confirmacion.Equals(
            "SI",
            StringComparison.OrdinalIgnoreCase))
        {
            if (inventario.Eliminar(codigo))
            {
                GuardarInventario();

                Console.WriteLine(
                    "Producto eliminado correctamente.");
            }
            else
            {
                Console.WriteLine(
                    "No se pudo eliminar el producto.");
            }
        }
        else
        {
            Console.WriteLine(
                "Eliminación cancelada.");
        }
    }

    // ========================================
    // CALCULAR VALOR DEL INVENTARIO
    // ========================================

    private void CalcularValorInventario()
    {
        double total = 0;

        CalcularValorRecursivo(
            inventario.raiz, ref total);

        Console.WriteLine(
            "\n--- VALOR DEL INVENTARIO ---");

        Console.WriteLine(
            "Valor total: Q" +
            total.ToString("0.00"));
    }

    private void CalcularValorRecursivo(
        NodoAVL nodo, ref double total)
    {
        if (nodo != null)
        {
            CalcularValorRecursivo(
                nodo.izquierdo, ref total);

            total += nodo.producto.precio *
                     nodo.producto.stock;

            CalcularValorRecursivo(
                nodo.derecho, ref total);
        }
    }

    // ========================================
    // REGISTRAR CLIENTE
    // ========================================

    private void RegistrarCliente()
    {
        Console.WriteLine(
            "\n--- REGISTRAR CLIENTE ---");

        string nombre = LeerTexto(
            "Nombre del cliente: ");

        Console.WriteLine(
            "\nTipo de cliente:");

        Console.WriteLine(
            "1. VIP (prioridad alta)");

        Console.WriteLine(
            "2. Adulto mayor (prioridad media)");

        Console.WriteLine(
            "3. Regular (prioridad normal)");

        int tipo = LeerEntero(
            "Seleccione el tipo (1-3): ",
            1, 3);

        // Cantidad de productos del cliente
        int cantidad = LeerEntero(
            "Cantidad de productos (1-1000): ",
            1, 1000);

        if (tipo == 1)
        {
            colaClientes.Agregar(
                nombre, "VIP", 1, cantidad);
        }
        else if (tipo == 2)
        {
            colaClientes.Agregar(
                nombre, "Adulto mayor", 2, cantidad);
        }
        else
        {
            colaClientes.Agregar(
                nombre, "Regular", 3, cantidad);
        }

        // Guardar también la hora y fecha
        GuardarClientes();

        Console.WriteLine(
            "\nCliente registrado en la cola correctamente.");

        Console.WriteLine(
            "Cantidad de productos: " + cantidad);

        Console.WriteLine(
            "Fecha y hora registradas automáticamente.");
    }

    // ========================================
    // ATENDER CLIENTE
    // ========================================

    private void AtenderCliente()
    {
        Console.WriteLine(
            "\n--- ATENDER CLIENTE ---");

        Cliente cliente =
            colaClientes.AtenderSiguiente();

        if (cliente == null)
        {
            Console.WriteLine(
                "No hay clientes en espera.");

            return;
        }

        // Quitar del archivo al cliente atendido
        GuardarClientes();

        string cantidadTexto =
            cliente.cantidadProductos == 0
            ? "No registrada"
            : cliente.cantidadProductos.ToString();

        string fechaTexto =
            cliente.fechaLlegada == DateTime.MinValue
            ? "No registrada"
            : cliente.fechaLlegada.ToString(
                "dd/MM/yyyy HH:mm:ss");

        Console.WriteLine(
            "Atendiendo a: " + cliente.nombre);

        Console.WriteLine(
            "Tipo de cliente: " + cliente.tipo);

        Console.WriteLine(
            "Prioridad: " + cliente.prioridad);

        Console.WriteLine(
            "Cantidad de productos: " + cantidadTexto);

        Console.WriteLine(
            "Fecha y hora de llegada: " + fechaTexto);

        Console.WriteLine(
            "Cliente retirado de la cola.");
    }

    // ========================================
    // ESTADISTICAS DEL SISTEMA
    // ========================================

    private void MostrarEstadisticas()
    {
        double total = 0;

        CalcularValorRecursivo(
            inventario.raiz, ref total);

        int productos =
            ContarProductos(inventario.raiz);

        Console.WriteLine(
            "\n--- ESTADÍSTICAS DE DON JUAN MARKET ---");

        Console.WriteLine(
            "Cantidad de productos distintos: " +
            productos);

        Console.WriteLine(
            "Valor total del inventario: Q" +
            total.ToString("0.00"));

        Console.WriteLine(
            "Clientes en espera: " +
            colaClientes.Cantidad);

        // Total de articulos de clientes en espera
        Console.WriteLine(
            "Artículos de clientes en espera: " +
            colaClientes.ContarArticulosEnEspera());

        Console.WriteLine(
            "Clientes VIP en espera: " +
            colaClientes.ContarTipo("VIP"));

        Console.WriteLine(
            "Adultos mayores en espera: " +
            colaClientes.ContarTipo("Adulto mayor"));

        Console.WriteLine(
            "Clientes regulares en espera: " +
            colaClientes.ContarTipo("Regular"));
    }

    // ========================================
    // CONTAR PRODUCTOS DEL AVL
    // ========================================

    private int ContarProductos(NodoAVL nodo)
    {
        if (nodo == null)
        {
            return 0;
        }

        return 1 +
            ContarProductos(nodo.izquierdo) +
            ContarProductos(nodo.derecho);
    }
}

