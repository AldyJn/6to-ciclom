datos <- read.csv("Datas_Laboratorio/ventas_empresa.csv",
                  encoding = "UTF-8", stringsAsFactors = FALSE)

cat("a) Primeras filas del dataset:\n")
print(head(datos))

cat("\nb) Dimensiones del dataset:\n")
cat("   Filas:", nrow(datos), "\n")
cat("   Columnas:", ncol(datos), "\n")

cat("\n   Valores nulos por columna:\n")
print(colSums(is.na(datos)))
cat("   Total de valores nulos:", sum(is.na(datos)), "\n")

cantidad <- tapply(datos$Cantidad, datos$Producto, sum)
cantidad <- sort(cantidad, decreasing = TRUE)

cat("\nc) Cantidad vendida por producto:\n")
print(cantidad)

png("capturas/ejercicio6_grafico.png", width = 900, height = 600)
bp <- barplot(cantidad,
              main = "Cantidad vendida por producto",
              xlab = "Producto",
              ylab = "Cantidad vendida",
              col = "steelblue",
              border = "black",
              ylim = c(0, max(cantidad) * 1.15),
              las = 1)
text(bp, cantidad, labels = cantidad, pos = 3)
dev.off()

cat("\nProducto con mayor cantidad vendida:", names(cantidad)[1],
    "con", cantidad[1], "unidades\n")
