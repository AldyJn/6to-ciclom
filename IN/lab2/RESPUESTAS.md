# Laboratorio 02 - Inteligencia de Negocios

Scripts: `ejercicio1.py` a `ejercicio5.py` (Python 3.13, pandas 3.0, matplotlib 3.10) y `ejercicio6.R` (R 4.6.1).
Capturas: carpeta `capturas/`.

## Ejercicio 1 - ventas.csv
- a) Primeras 5 filas: Laptop, Mouse, Teclado, Monitor y Laptop.
- b) 10 filas y 4 columnas. Producto y Categoria son de texto, Cantidad es int64 y Precio es float64 (float porque tiene un valor faltante).
- c) Existe 1 valor nulo, ubicado en la columna Precio. Las demas columnas estan completas.

## Ejercicio 2 - clientes.csv
- a) 4 valores nulos: Ciudad (1), Edad (2) y Correo (1).
- b) Existen 2 registros duplicados.
- c) Los duplicados corresponden a C002 (Luis Ramos, Arequipa) y C007 (Lucia Vega, Trujillo), cada uno repetido una vez.

## Ejercicio 3 - productos.csv
- a) Se creo la columna Importe = Cantidad * Precio.
- b) Importe total por producto: Laptop 12500, Monitor 5950, Silla 3200, Teclado 1440, Mouse 810.
- c) El producto con mayor importe total es **Laptop**, con S/ 12 500, pese a vender pocas unidades, debido a su precio unitario alto.

## Ejercicio 4 - ventas.csv (grafico de barras)
- a) Cantidad total vendida: Mouse 33, Papel A4 25, Teclado 12, Silla 10, Laptop 8, Monitor 7, Escritorio 6, Impresora 4.
- b) Grafico de barras verticales en `capturas/ejercicio4_grafico.png`, con titulo y ejes rotulados.
- c) El producto con mayor cantidad vendida es **Mouse**, con 33 unidades.

## Ejercicio 5 - categorias.csv (grafico circular)
- a) Total de ventas por categoria: Tecnologia 28 300, Mobiliario 9 500, Accesorios 7 500, Oficina 4 700. Total general: 50 000.
- b) Grafico circular en `capturas/ejercicio5_grafico.png` con el porcentaje de cada categoria.
- c) La categoria con mayor participacion es **Tecnologia**, con 56.6 % del total de ventas, mas que la suma de las otras tres.

## Ejercicio 6 - ventas_empresa.csv en R
- a) Se importo con `read.csv()` y se mostraron las primeras filas con `head()`.
- b) 12 filas y 4 columnas. Hay 2 valores nulos, ambos en la columna Precio.
- c) Grafico de barras en `capturas/ejercicio6_grafico.png`. El producto con mayor cantidad vendida es **Mouse**, con 32 unidades, seguido de Papel A4 con 30.

## Conclusiones
1. Pandas resuelve todo el flujo de importar, revisar, limpiar y transformar con instrucciones puntuales: read_csv(), info(), isnull().sum(), duplicated() y groupby() bastaron para los cinco primeros ejercicios, y matplotlib genero los graficos sobre esos mismos resultados sin pasos intermedios.
2. Python y R llegan al mismo resultado con sintaxis distinta: lo que en pandas se hace con groupby().sum(), en R se resuelve con tapply(), y read_csv()/read.csv() o isnull().sum()/colSums(is.na()) son equivalentes, por lo que elegir el lenguaje depende del entorno de trabajo y no del analisis.
