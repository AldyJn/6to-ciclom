#!/bin/bash
BASE="${1:-https://sixto-ciclom.onrender.com}"
OUT="$(dirname "$0")/resultados.md"
H='Content-Type: application/json'
n=0

req() {
  local titulo="$1" esperado="$2" metodo="$3" ruta="$4" cuerpo="$5"
  n=$((n+1))
  local resp
  if [ -n "$cuerpo" ]; then
    resp=$(curl -s -m 60 -w '\n%{http_code}' -X "$metodo" -H "$H" -d "$cuerpo" "$BASE$ruta")
  else
    resp=$(curl -s -m 60 -w '\n%{http_code}' -X "$metodo" "$BASE$ruta")
  fi
  local codigo="${resp##*$'\n'}"
  local cuerpo_resp="${resp%$'\n'*}"
  local estado="OK"
  [ "$codigo" != "$esperado" ] && estado="FALLO"
  {
    echo "## $n. $titulo"
    echo
    echo "- Peticion: \`$metodo $ruta\`"
    [ -n "$cuerpo" ] && echo "- Cuerpo: \`$cuerpo\`"
    echo "- Esperado: $esperado | Obtenido: **$codigo** | $estado"
    echo
    echo '```json'
    echo "${cuerpo_resp:0:700}"
    echo '```'
    echo
  } >> "$OUT"
  echo "$n $metodo $ruta -> $codigo (esperado $esperado) $estado"
  LAST="$cuerpo_resp"
}

{
  echo "# Pruebas sobre la API publicada"
  echo
  echo "URL base: $BASE"
  echo "Fecha: $(date '+%Y-%m-%d %H:%M')"
  echo
} > "$OUT"

DNI="9$(date +%s | tail -c 8)"

req "Listar propietarios" 200 GET /api/propietarios
req "Filtrar propietarios por nombre" 200 GET "/api/propietarios?nombre=Lucia"
req "Obtener propietario 1" 200 GET /api/propietarios/1
req "Mascotas del propietario 1 (ruta anidada)" 200 GET /api/propietarios/1/mascotas
req "Propietario inexistente" 404 GET /api/propietarios/9999
req "Mascotas de propietario inexistente" 404 GET /api/propietarios/9999/mascotas
req "Listar mascotas" 200 GET /api/mascotas
req "Filtrar mascotas por especie, activa y peso minimo" 200 GET "/api/mascotas?especie=Perro&activa=true&pesoMin=5"
req "Obtener mascota 1" 200 GET /api/mascotas/1
req "Mascota inexistente" 404 GET /api/mascotas/9999

req "Crear propietario" 201 POST /api/propietarios "{\"nombre\":\"Prueba Nube\",\"dni\":\"$DNI\",\"telefono\":\"900111222\",\"email\":\"prueba@correo.com\"}"
PID=$(echo "$LAST" | sed -n 's/.*"id":\([0-9]*\).*/\1/p')
req "Crear propietario con DNI repetido" 409 POST /api/propietarios "{\"nombre\":\"Otro\",\"dni\":\"$DNI\",\"telefono\":\"900111222\"}"
req "Crear propietario con datos invalidos" 400 POST /api/propietarios '{"nombre":"A","dni":"123","telefono":"1"}'
req "Actualizar propietario" 204 PUT "/api/propietarios/$PID" "{\"id\":$PID,\"nombre\":\"Prueba Nube Editado\",\"dni\":\"$DNI\",\"telefono\":\"900111222\",\"email\":\"prueba@correo.com\"}"
req "Actualizar propietario con id distinto en ruta y cuerpo" 400 PUT "/api/propietarios/$PID" "{\"id\":9999,\"nombre\":\"Prueba\",\"dni\":\"$DNI\",\"telefono\":\"900111222\"}"
req "Actualizar propietario inexistente" 404 PUT /api/propietarios/9999 '{"id":9999,"nombre":"Prueba","dni":"12345678","telefono":"900111222"}'

req "Crear mascota" 201 POST /api/mascotas "{\"nombre\":\"Toby\",\"especie\":\"Perro\",\"raza\":\"Beagle\",\"pesoKg\":10.5,\"fechaNacimiento\":\"2022-01-10\",\"activa\":true,\"propietarioId\":$PID}"
MID=$(echo "$LAST" | sed -n 's/.*"id":\([0-9]*\).*/\1/p')
req "Crear mascota con propietario inexistente" 400 POST /api/mascotas '{"nombre":"Fantasma","especie":"Gato","pesoKg":3,"fechaNacimiento":"2022-01-10","activa":true,"propietarioId":9999}'
req "Crear mascota con nombre repetido para el mismo propietario" 409 POST /api/mascotas "{\"nombre\":\"Toby\",\"especie\":\"Perro\",\"pesoKg\":8,\"fechaNacimiento\":\"2022-01-10\",\"activa\":true,\"propietarioId\":$PID}"
req "Crear mascota con peso invalido" 400 POST /api/mascotas "{\"nombre\":\"Pesado\",\"especie\":\"Perro\",\"pesoKg\":-5,\"fechaNacimiento\":\"2022-01-10\",\"activa\":true,\"propietarioId\":$PID}"
req "Actualizar mascota" 204 PUT "/api/mascotas/$MID" "{\"id\":$MID,\"nombre\":\"Toby\",\"especie\":\"Perro\",\"raza\":\"Beagle\",\"pesoKg\":11.2,\"fechaNacimiento\":\"2022-01-10\",\"activa\":true,\"propietarioId\":$PID}"
req "Actualizar mascota con id distinto en ruta y cuerpo" 400 PUT "/api/mascotas/$MID" "{\"id\":9999,\"nombre\":\"Toby\",\"especie\":\"Perro\",\"pesoKg\":11,\"fechaNacimiento\":\"2022-01-10\",\"activa\":true,\"propietarioId\":$PID}"
req "Actualizar mascota inexistente" 404 PUT /api/mascotas/9999 "{\"id\":9999,\"nombre\":\"Toby\",\"especie\":\"Perro\",\"pesoKg\":11,\"fechaNacimiento\":\"2022-01-10\",\"activa\":true,\"propietarioId\":$PID}"

req "Eliminar propietario que tiene mascotas" 409 DELETE "/api/propietarios/$PID"
req "Eliminar mascota" 204 DELETE "/api/mascotas/$MID"
req "Eliminar mascota ya eliminada" 404 DELETE "/api/mascotas/$MID"
req "Eliminar propietario sin mascotas" 204 DELETE "/api/propietarios/$PID"
req "Eliminar propietario ya eliminado" 404 DELETE "/api/propietarios/$PID"
