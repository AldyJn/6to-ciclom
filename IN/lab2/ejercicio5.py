import io
import contextlib
import pandas as pd
import matplotlib
matplotlib.use("Agg")
import matplotlib.pyplot as plt
from render_captura import captura

CODIGO = """import pandas as pd
import matplotlib.pyplot as plt

df = pd.read_csv("Datas_Laboratorio/categorias.csv", encoding="utf-8-sig")

totales = df.groupby("Categoria")["Venta"].sum().sort_values(ascending=False)
print(totales)

plt.pie(totales.values, labels=totales.index, autopct="%1.1f%%")
plt.title("Participacion de ventas por categoria")
plt.show()"""

buf = io.StringIO()
with contextlib.redirect_stdout(buf):
    df = pd.read_csv("Datas_Laboratorio/categorias.csv", encoding="utf-8-sig")

    print("a) Total de ventas por categoria:")
    totales = df.groupby("Categoria")["Venta"].sum().sort_values(ascending=False)
    print(totales)
    print("Total general de ventas:", int(totales.sum()))

    print("\nPorcentaje de participacion por categoria:")
    print((totales / totales.sum() * 100).round(2))

    print("\nb) Grafico circular generado -> capturas/ejercicio5_grafico.png")

    print("\nc) Categoria con la mayor participacion en las ventas:")
    print("   ", totales.idxmax(), "->", int(totales.max()),
          "(", round(float(totales.max() / totales.sum() * 100), 2), "% )")

salida = buf.getvalue()
print(salida)

fig, ax = plt.subplots(figsize=(7, 6))
colores = ["#4c72b0", "#dd8452", "#55a868", "#c44e52", "#8172b3"]
ax.pie(totales.values, labels=totales.index, autopct="%1.1f%%", startangle=90,
       colors=colores[:len(totales)],
       wedgeprops={"edgecolor": "white", "linewidth": 1.5})
ax.set_title("Participacion de ventas por categoria", fontsize=13, weight="bold")
ax.axis("equal")
fig.tight_layout()
fig.savefig("capturas/ejercicio5_grafico.png", dpi=130)
plt.close(fig)
print("[captura guardada] capturas/ejercicio5_grafico.png")

captura("EJERCICIO 5 - Ventas por categoria (categorias.csv)", CODIGO, salida,
        "capturas/ejercicio5.png")
