import io
import contextlib
import pandas as pd
from render_captura import captura

CODIGO = """import pandas as pd

df = pd.read_csv("Datas_Laboratorio/clientes.csv", encoding="utf-8-sig")

print(df.isnull().sum())

print(df.duplicated().sum())

print(df[df.duplicated(keep=False)])"""

buf = io.StringIO()
with contextlib.redirect_stdout(buf):
    df = pd.read_csv("Datas_Laboratorio/clientes.csv", encoding="utf-8-sig")

    print("Dataset clientes.csv ->", df.shape[0], "filas y", df.shape[1], "columnas\n")

    print("a) Cantidad de valores nulos por columna:")
    print(df.isnull().sum())
    print("Total de valores nulos:", int(df.isnull().sum().sum()))

    print("\nb) Cantidad de registros duplicados:", int(df.duplicated().sum()))

    print("\nc) Registros duplicados (se muestran todas las coincidencias):")
    print(df[df.duplicated(keep=False)].sort_values("ID_Cliente"))

salida = buf.getvalue()
print(salida)
captura("EJERCICIO 2 - Nulos y duplicados en clientes.csv", CODIGO, salida,
        "capturas/ejercicio2.png")
