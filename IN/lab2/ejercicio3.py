import io
import contextlib
import pandas as pd
from render_captura import captura

CODIGO = """import pandas as pd

df = pd.read_csv("Datas_Laboratorio/productos.csv", encoding="utf-8-sig")

df["Importe"] = df["Cantidad"] * df["Precio"]
print(df)

totales = df.groupby("Producto")["Importe"].sum()
print(totales)

print(totales.idxmax(), totales.max())"""

buf = io.StringIO()
with contextlib.redirect_stdout(buf):
    df = pd.read_csv("Datas_Laboratorio/productos.csv", encoding="utf-8-sig")

    print("a) Dataset con la nueva columna Importe (Cantidad * Precio):")
    df["Importe"] = df["Cantidad"] * df["Precio"]
    print(df)

    print("\nb) Importe total por producto:")
    totales = df.groupby("Producto")["Importe"].sum().sort_values(ascending=False)
    print(totales)

    print("\nc) Producto con el mayor importe total de ventas:")
    print("   ", totales.idxmax(), "-> S/", round(float(totales.max()), 2))

salida = buf.getvalue()
print(salida)
captura("EJERCICIO 3 - Columna Importe y agrupacion (productos.csv)", CODIGO, salida,
        "capturas/ejercicio3.png")
