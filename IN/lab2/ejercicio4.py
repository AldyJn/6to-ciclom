import io
import contextlib
import pandas as pd
import matplotlib
matplotlib.use("Agg")
import matplotlib.pyplot as plt
from render_captura import captura

CODIGO = """import pandas as pd
import matplotlib.pyplot as plt

df = pd.read_csv("Datas_Laboratorio/ventas.csv", encoding="utf-8-sig")

cantidad = df.groupby("Producto")["Cantidad"].sum().sort_values(ascending=False)
print(cantidad)

plt.bar(cantidad.index, cantidad.values, color="#4c72b0")
plt.title("Cantidad vendida por producto")
plt.xlabel("Producto")
plt.ylabel("Cantidad vendida")
plt.show()"""

buf = io.StringIO()
with contextlib.redirect_stdout(buf):
    df = pd.read_csv("Datas_Laboratorio/ventas.csv", encoding="utf-8-sig")

    print("a) Cantidad total vendida por producto:")
    cantidad = df.groupby("Producto")["Cantidad"].sum().sort_values(ascending=False)
    print(cantidad)

    print("\nb) Grafico de barras verticales generado -> capturas/ejercicio4_grafico.png")

    print("\nc) Producto con la mayor cantidad vendida:")
    print("   ", cantidad.idxmax(), "con", int(cantidad.max()), "unidades")

salida = buf.getvalue()
print(salida)

fig, ax = plt.subplots(figsize=(8, 5))
barras = ax.bar(cantidad.index, cantidad.values, color="#4c72b0", edgecolor="black")
ax.bar_label(barras, padding=3)
ax.set_title("Cantidad vendida por producto", fontsize=13, weight="bold")
ax.set_xlabel("Producto")
ax.set_ylabel("Cantidad vendida")
ax.grid(axis="y", linestyle="--", alpha=0.4)
ax.set_axisbelow(True)
fig.tight_layout()
fig.savefig("capturas/ejercicio4_grafico.png", dpi=130)
plt.close(fig)
print("[captura guardada] capturas/ejercicio4_grafico.png")

captura("EJERCICIO 4 - Cantidad vendida por producto (ventas.csv)", CODIGO, salida,
        "capturas/ejercicio4.png")
