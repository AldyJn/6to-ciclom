import io
import contextlib
import pandas as pd
from render_captura import captura

CODIGO = """import pandas as pd

df = pd.read_csv("Datas_Laboratorio/ventas.csv", encoding="utf-8-sig")

print(df.head(5))

df.info()

print(df.isnull().sum())"""

buf = io.StringIO()
with contextlib.redirect_stdout(buf):
    df = pd.read_csv("Datas_Laboratorio/ventas.csv", encoding="utf-8-sig")

    print("a) Primeras cinco filas:")
    print(df.head(5))

    print("\nb) Informacion general del dataset:")
    df.info()

    print("\nc) Cantidad de valores nulos por columna:")
    print(df.isnull().sum())
    print("\nTotal de valores nulos en el dataset:", int(df.isnull().sum().sum()))

salida = buf.getvalue()
print(salida)
captura("EJERCICIO 1 - Importar y revisar ventas.csv", CODIGO, salida,
        "capturas/ejercicio1.png")
