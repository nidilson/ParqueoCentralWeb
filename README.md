# Parqueo Central Web

Sistema web para la gestión y administración de un estacionamiento. La aplicación permite registrar vehículos, administrar espacios de estacionamiento, controlar las entradas y salidas de vehículos y consultar el historial de movimientos realizados.

El sistema también permite identificar al operador que realiza cada operación mediante el uso de sesiones y proporciona una interfaz web responsive para facilitar su uso desde diferentes dispositivos.

---

## Descripción

El sistema permite gestionar las principales operaciones de un estacionamiento:

* Registro, consulta, edición y eliminación de vehículos.
* Registro, consulta, edición y eliminación de espacios de estacionamiento.
* Registro de entradas de vehículos.
* Registro de salidas de vehículos.
* Control del estado de los espacios de estacionamiento.
* Cálculo del tiempo de permanencia.
* Registro del monto cobrado.
* Consulta del historial de movimientos.
* Identificación del operador que registra cada movimiento.

---

## Tecnologías utilizadas

### Backend

* **ASP.NET MVC 5**
* **.NET Framework**
* **C#**
* **Entity Framework 6**
* **LINQ**
* **SQL Server 2022**

### Frontend

* **HTML5**
* **CSS3**
* **JavaScript**
* **jQuery**
* **Bootstrap**
* **Bootstrap Icons**
* **DataTables**

### Herramientas y entorno de desarrollo

* **Visual Studio**
* **NuGet**
* **SQL Server Management Studio (SSMS)**
* **Git** (opcional, para control de versiones)

---

## Requisitos para ejecutar el proyecto

Antes de ejecutar el sistema, es necesario contar con los siguientes componentes instalados:

### 1. Visual Studio

Se requiere tener instalado **Visual Studio** con las herramientas necesarias para desarrollar aplicaciones web con ASP.NET MVC y .NET Framework.

---

### 2. SQL Server 2022

Es necesario tener instalado **SQL Server 2022** para ejecutar la base de datos del sistema.

La instancia de SQL Server puede ser:

* SQL Server 2022 Developer.
* SQL Server 2022 Express.
* SQL Server 2022 Standard o Enterprise.

---

### 3. SQL Server Management Studio

Se recomienda instalar **SQL Server Management Studio (SSMS)** para Administrar la base de datos.

---

### 4. Navegador web

Se recomienda utilizar una versión actualizada de Google Chrome.

---

## Estructura de la base de datos

El sistema utiliza una base de datos relacional compuesta principalmente por las siguientes tablas:

### Vehiculo

Almacena la información de los vehículos registrados.

### EspacioEstacionamiento

Almacena la información de los espacios disponibles en el estacionamiento.

### MovimientoEstacionamiento

Registra las entradas y salidas de los vehículos y mantiene el historial de uso del estacionamiento.

La tabla `MovimientoEstacionamiento` mantiene relaciones con las tablas `Vehiculo` y `EspacioEstacionamiento` mediante claves foráneas.

---

# Instalación

## 1. Clonar o descargar el proyecto

Si el proyecto se encuentra en un repositorio Git, clónelo utilizando:

```bash
git clone URL_DEL_REPOSITORIO
```

También es posible descargar el proyecto directamente y extraer los archivos en una carpeta local.

---

## 2. Abrir el proyecto en Visual Studio

Abra el archivo de solución:

```text
ParqueoCentralWeb.sln
```

utilizando Visual Studio.

Una vez abierto el proyecto:

1. Espere a que Visual Studio cargue la solución.
2. Verifique que no existan errores de compilación.
3. Restaure los paquetes NuGet si es necesario.

Para restaurar los paquetes NuGet puede utilizar:

```text
Tools → NuGet Package Manager → Package Manager Settings
```

o restaurar los paquetes desde la solución según la configuración de Visual Studio.

---

## Creación y publicación de la base de datos

El repositorio incluye un proyecto de base de datos de Visual Studio. Este proyecto contiene la definición de la base de datos, incluyendo las tablas, relaciones y demás objetos necesarios para el funcionamiento del sistema.

La base de datos debe publicarse utilizando el proyecto de base de datos incluido en la solución.

---

### 1. Abrir la solución

Abra el archivo de solución del proyecto:

```text
ParqueoCentralWeb.sln
```

utilizando Visual Studio.

La solución debe incluir tanto el proyecto web como el proyecto correspondiente a la base de datos tal como se ve en la imagen.
![Solucion de la base de datos](/README%20IMAGES/Soluciones.png)

---

### 2. Localizar el proyecto de base de datos

En el **Solution Explorer**, localice el proyecto de base de datos.

Este proyecto contiene la estructura de la base de datos, incluyendo las tablas:

```text
Vehiculo
EspacioEstacionamiento
MovimientoEstacionamiento
```

y las relaciones entre ellas.

---

### 3. Publicar la base de datos

Haga clic derecho sobre el proyecto de base de datos y seleccione:

```text
Publish...
```

Se abrirá el asistente de publicación de la base de datos.

---

### 4. Configurar la conexión de destino

En el asistente de publicación, configure la conexión hacia la instancia de SQL Server 2022 donde desea crear la base de datos.

Por ejemplo:

```text
localhost
```

```text
.\SQLEXPRESS
```

o la instancia correspondiente a la instalación local de SQL Server.

Posteriormente, seleccione o configure la base de datos de destino:

```text
ParqueoCentralWebDB
```

---

### 5. Ejecutar la publicación

Una vez configurada la conexión:

1. Verifique que la instancia de SQL Server sea correcta.
2. Verifique que el nombre de la base de datos sea `ParqueoCentral`.
3. Revise la configuración de publicación.
4. Presione el botón:

```text
Publish
```

Visual Studio ejecutará el proceso de publicación y creará o actualizará la base de datos en la instancia seleccionada.

Al finalizar, puede verificar la base de datos desde **SQL Server Management Studio**.

---


# Configuración de la conexión a la base de datos

Una vez publicada la base de datos, es necesario configurar la aplicación web para que utilice la instancia y la base de datos correctas.

La conexión se configura en el archivo:

```text
Web.config
```

Dentro de este archivo se debe localizar la sección:

```xml
<connectionStrings>
```

La cadena de conexión debe apuntar a la misma instancia de SQL Server y a la misma base de datos utilizadas durante la publicación.

Por ejemplo:

```xml
<connectionStrings>
    <add name="ParqueoCentralDBEntities"
         connectionString="metadata=res://*/Models.ParqueoCentralModel.csdl|res://*/Models/ParqueoCentralModel.ssdl|res://*/Models/ParqueoCentralModel.msl;
         provider=System.Data.SqlClient;
         provider connection string=&quot;
         data source=.\SQLEXPRESS;
         initial catalog=ParqueoCentralWeb;
         integrated security=True;
         multipleactiveresultsets=True;
         trustservercertificate=True;
         app=EntityFramework&quot;"
         providerName="System.Data.EntityClient" />
</connectionStrings>
```

Los valores más importantes que deben revisarse son:

```text
data source
```

y:

```text
initial catalog
```

Por ejemplo:

```text
data source=.\SQLEXPRESS;
initial catalog=ParqueoCentral;
```

La propiedad `data source` debe coincidir con la instancia de SQL Server utilizada para publicar la base de datos.


El valor:

```text
initial catalog=ParqueoCentral;
```

debe coincidir con el nombre de la base de datos configurado durante el proceso de publicación.

---

## Autenticación de SQL Server

Si se utiliza autenticación integrada de Windows, la cadena de conexión debe utilizar:

```text
integrated security=True;
```

En este caso, el usuario de Windows que ejecuta la aplicación debe tener los permisos necesarios para acceder a la base de datos.

Si se utiliza autenticación mediante usuario y contraseña de SQL Server, la cadena de conexión puede configurarse de la siguiente manera:

```text
data source=localhost;
initial catalog=ParqueoCentral;
user id=USUARIO;
password=CONTRASEÑA;
```

---

## Resumen del proceso

El orden recomendado para configurar el proyecto es:

1. Abrir la solución en Visual Studio.
2. Verificar que el proyecto de base de datos esté incluido.
3. Publicar el proyecto de base de datos utilizando la opción `Publish`.
4. Seleccionar la instancia de SQL Server 2022.
5. Crear o actualizar la base de datos `ParqueoCentral`.
6. Verificar la publicación desde SQL Server Management Studio.
7. Modificar la cadena de conexión del archivo `Web.config`.
8. Confirmar que `data source` e `initial catalog` coincidan con la instancia y la base de datos publicadas.
9. Compilar y ejecutar el proyecto web.

# 6. Restaurar paquetes NuGet

El proyecto utiliza diferentes paquetes para su funcionamiento, entre ellos:

* Entity Framework.
* EntityFramework.SqlServer.
* EntityFramework.Tools.

Si los paquetes no se restauran automáticamente, pueden restaurarse desde Visual Studio mediante el administrador de paquetes NuGet.

También es posible utilizar la consola del administrador de paquetes:

```powershell
Update-Package -reinstall
```

---

# 7. Compilar el proyecto

Antes de ejecutar la aplicación, compile la solución:

```text
Build → Build Solution
```

Verifique que la compilación finalice correctamente.

---

# 8. Ejecutar la aplicación

Para ejecutar el proyecto:

1. Seleccione el proyecto web como proyecto de inicio.
2. Seleccione IIS Express o el servidor configurado.
3. Presione:

```text
IIS Express (Google Chrome)
```

La aplicación se abrirá en el navegador.

---

# Nidilson Rodríguez Carpio

Para el desarrollo de esta tarea utilicé las herramientas Chat-GPT y Google Stitch como apoyo para la generación de código y de interfaces gráficas. El código, la lógica implementada y el documento final fueron revisados, comprendidos y ajustados por mi persona.

Puede observar el video de presentación en el siguiente enlace
[Video Presentacion](https://unedcr-my.sharepoint.com/:v:/g/personal/nidilson_rodriguez_uned_cr/IQBLuEXM1k-wSKfE4Ku4lK0yARQFwN9Ec8X3BNOh-LAJyCE?nav=eyJyZWZlcnJhbEluZm8iOnsicmVmZXJyYWxBcHAiOiJPbmVEcml2ZUZvckJ1c2luZXNzIiwicmVmZXJyYWxBcHBQbGF0Zm9ybSI6IldlYiIsInJlZmVycmFsTW9kZSI6InZpZXciLCJyZWZlcnJhbFZpZXciOiJNeUZpbGVzTGlua0NvcHkifX0&e=rXlEY5)

