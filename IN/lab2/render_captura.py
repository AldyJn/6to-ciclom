import matplotlib
matplotlib.use("Agg")
import matplotlib.pyplot as plt


def captura(titulo, codigo, salida, archivo):
    lineas = ["", ""]
    lineas.append(">>> CODIGO")
    lineas += codigo.strip("\n").split("\n")
    lineas.append("")
    lineas.append(">>> SALIDA")
    lineas += salida.rstrip("\n").split("\n")

    ancho = max([len(l) for l in lineas] + [len(titulo)])
    fig_w = max(8.0, min(16.0, ancho * 0.105 + 0.8))
    fig_h = max(3.0, len(lineas) * 0.235 + 1.0)

    fig = plt.figure(figsize=(fig_w, fig_h), facecolor="#1e1e1e")
    ax = fig.add_axes([0, 0, 1, 1])
    ax.axis("off")
    ax.set_facecolor("#1e1e1e")
    ax.text(0.015, 0.975, titulo, color="#4ec9b0", fontsize=12,
            family="DejaVu Sans", weight="bold", va="top", parse_math=False)

    y = 0.975 - (1.0 / len(lineas)) * 0.55
    paso = 0.92 / (len(lineas) + 2)
    for i, l in enumerate(lineas):
        if l.startswith(">>> "):
            color, w = "#dcdcaa", "bold"
        else:
            color, w = "#d4d4d4", "normal"
        ax.text(0.015, y - i * paso, l, color=color, fontsize=9.5,
                family="DejaVu Sans Mono", va="top", weight=w, parse_math=False)

    fig.savefig(archivo, dpi=130, facecolor="#1e1e1e")
    plt.close(fig)
    print("[captura guardada] " + archivo)
