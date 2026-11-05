# Pruebas sobre la API publicada

URL base: https://sixto-ciclom.onrender.com
Fecha: 2026-11-04 20:22

## 1. Listar propietarios

- Peticion: `GET /api/propietarios`
- Esperado: 200 | Obtenido: **200** | OK

```json
[{"id":1,"nombre":"Carlos Ramirez","dni":"45678123","telefono":"987654321","email":"carlos@correo.com"},{"id":3,"nombre":"Jorge Salazar","dni":"47891234","telefono":"956781234","email":null},{"id":2,"nombre":"Lucia Fernandez","dni":"40123456","telefono":"912345678","email":"lucia@correo.com"}]
```

## 2. Filtrar propietarios por nombre

- Peticion: `GET /api/propietarios?nombre=Lucia`
- Esperado: 200 | Obtenido: **200** | OK

```json
[{"id":2,"nombre":"Lucia Fernandez","dni":"40123456","telefono":"912345678","email":"lucia@correo.com"}]
```

## 3. Obtener propietario 1

- Peticion: `GET /api/propietarios/1`
- Esperado: 200 | Obtenido: **200** | OK

```json
{"id":1,"nombre":"Carlos Ramirez","dni":"45678123","telefono":"987654321","email":"carlos@correo.com"}
```

## 4. Mascotas del propietario 1 (ruta anidada)

- Peticion: `GET /api/propietarios/1/mascotas`
- Esperado: 200 | Obtenido: **200** | OK

```json
[{"id":2,"nombre":"Michi","especie":"Gato","raza":"Siames","pesoKg":4.20,"fechaNacimiento":"2021-07-01","activa":true,"propietarioId":1,"propietario":null},{"id":1,"nombre":"Rocky","especie":"Perro","raza":"Labrador","pesoKg":28.50,"fechaNacimiento":"2019-03-12","activa":true,"propietarioId":1,"propietario":null}]
```

## 5. Propietario inexistente

- Peticion: `GET /api/propietarios/9999`
- Esperado: 404 | Obtenido: **404** | OK

```json
{"type":"https://tools.ietf.org/html/rfc9110#section-15.5.5","title":"Not Found","status":404,"traceId":"00-75d1c0c99adc7d24884b7db682dd5068-d0a98664d1c0e002-00"}
```

## 6. Mascotas de propietario inexistente

- Peticion: `GET /api/propietarios/9999/mascotas`
- Esperado: 404 | Obtenido: **404** | OK

```json
{"type":"https://tools.ietf.org/html/rfc9110#section-15.5.5","title":"Not Found","status":404,"traceId":"00-ad7f268d00b9ab8f11af1203c603981a-48247d5a68a13fdd-00"}
```

## 7. Listar mascotas

- Peticion: `GET /api/mascotas`
- Esperado: 200 | Obtenido: **200** | OK

```json
[{"id":3,"nombre":"Luna","especie":"Perro","raza":"Poodle","pesoKg":6.80,"fechaNacimiento":"2020-11-23","activa":true,"propietarioId":2,"propietario":{"id":2,"nombre":"Lucia Fernandez","dni":"40123456","telefono":"912345678","email":"lucia@correo.com"}},{"id":2,"nombre":"Michi","especie":"Gato","raza":"Siames","pesoKg":4.20,"fechaNacimiento":"2021-07-01","activa":true,"propietarioId":1,"propietario":{"id":1,"nombre":"Carlos Ramirez","dni":"45678123","telefono":"987654321","email":"carlos@correo.com"}},{"id":4,"nombre":"Pelusa","especie":"Conejo","raza":null,"pesoKg":2.10,"fechaNacimiento":"2022-05-15","activa":false,"propietarioId":3,"propietario":{"id":3,"nombre":"Jorge Salazar","dni":"4789
```

## 8. Filtrar mascotas por especie, activa y peso minimo

- Peticion: `GET /api/mascotas?especie=Perro&activa=true&pesoMin=5`
- Esperado: 200 | Obtenido: **200** | OK

```json
[{"id":3,"nombre":"Luna","especie":"Perro","raza":"Poodle","pesoKg":6.80,"fechaNacimiento":"2020-11-23","activa":true,"propietarioId":2,"propietario":{"id":2,"nombre":"Lucia Fernandez","dni":"40123456","telefono":"912345678","email":"lucia@correo.com"}},{"id":1,"nombre":"Rocky","especie":"Perro","raza":"Labrador","pesoKg":28.50,"fechaNacimiento":"2019-03-12","activa":true,"propietarioId":1,"propietario":{"id":1,"nombre":"Carlos Ramirez","dni":"45678123","telefono":"987654321","email":"carlos@correo.com"}}]
```

## 9. Obtener mascota 1

- Peticion: `GET /api/mascotas/1`
- Esperado: 200 | Obtenido: **200** | OK

```json
{"id":1,"nombre":"Rocky","especie":"Perro","raza":"Labrador","pesoKg":28.50,"fechaNacimiento":"2019-03-12","activa":true,"propietarioId":1,"propietario":{"id":1,"nombre":"Carlos Ramirez","dni":"45678123","telefono":"987654321","email":"carlos@correo.com"}}
```

## 10. Mascota inexistente

- Peticion: `GET /api/mascotas/9999`
- Esperado: 404 | Obtenido: **404** | OK

```json
{"type":"https://tools.ietf.org/html/rfc9110#section-15.5.5","title":"Not Found","status":404,"traceId":"00-358eadd2c72ccb6f19f78cb699aa4213-de07540aac71476a-00"}
```

## 11. Crear propietario

- Peticion: `POST /api/propietarios`
- Cuerpo: `{"nombre":"Prueba Nube","dni":"93841752","telefono":"900111222","email":"prueba@correo.com"}`
- Esperado: 201 | Obtenido: **201** | OK

```json
{"id":4,"nombre":"Prueba Nube","dni":"93841752","telefono":"900111222","email":"prueba@correo.com"}
```

## 12. Crear propietario con DNI repetido

- Peticion: `POST /api/propietarios`
- Cuerpo: `{"nombre":"Otro","dni":"93841752","telefono":"900111222"}`
- Esperado: 409 | Obtenido: **409** | OK

```json
{"mensaje":"Ya existe un propietario con ese DNI"}
```

## 13. Crear propietario con datos invalidos

- Peticion: `POST /api/propietarios`
- Cuerpo: `{"nombre":"A","dni":"123","telefono":"1"}`
- Esperado: 400 | Obtenido: **400** | OK

```json
{"type":"https://tools.ietf.org/html/rfc9110#section-15.5.1","title":"One or more validation errors occurred.","status":400,"errors":{"Dni":["The field Dni must be a string with a minimum length of 8 and a maximum length of 8.","El DNI debe tener 8 digitos"],"Nombre":["The field Nombre must be a string with a minimum length of 3 and a maximum length of 100."],"Telefono":["The field Telefono must be a string with a minimum length of 7 and a maximum length of 15."]},"traceId":"00-0bcb3c0916969b7b53ecd473b3eebb6c-61bbc3fd33555fd8-00"}
```

## 14. Actualizar propietario

- Peticion: `PUT /api/propietarios/4`
- Cuerpo: `{"id":4,"nombre":"Prueba Nube Editado","dni":"93841752","telefono":"900111222","email":"prueba@correo.com"}`
- Esperado: 204 | Obtenido: **204** | OK

```json

```

## 15. Actualizar propietario con id distinto en ruta y cuerpo

- Peticion: `PUT /api/propietarios/4`
- Cuerpo: `{"id":9999,"nombre":"Prueba","dni":"93841752","telefono":"900111222"}`
- Esperado: 400 | Obtenido: **400** | OK

```json
{"mensaje":"El id de la ruta no coincide con el del cuerpo"}
```

## 16. Actualizar propietario inexistente

- Peticion: `PUT /api/propietarios/9999`
- Cuerpo: `{"id":9999,"nombre":"Prueba","dni":"12345678","telefono":"900111222"}`
- Esperado: 404 | Obtenido: **404** | OK

```json
{"type":"https://tools.ietf.org/html/rfc9110#section-15.5.5","title":"Not Found","status":404,"traceId":"00-02156b124552345d1adcad91bcd1455f-82fcc879fd12cbdc-00"}
```

## 17. Crear mascota

- Peticion: `POST /api/mascotas`
- Cuerpo: `{"nombre":"Toby","especie":"Perro","raza":"Beagle","pesoKg":10.5,"fechaNacimiento":"2022-01-10","activa":true,"propietarioId":4}`
- Esperado: 201 | Obtenido: **201** | OK

```json
{"id":5,"nombre":"Toby","especie":"Perro","raza":"Beagle","pesoKg":10.5,"fechaNacimiento":"2022-01-10","activa":true,"propietarioId":4,"propietario":null}
```

## 18. Crear mascota con propietario inexistente

- Peticion: `POST /api/mascotas`
- Cuerpo: `{"nombre":"Fantasma","especie":"Gato","pesoKg":3,"fechaNacimiento":"2022-01-10","activa":true,"propietarioId":9999}`
- Esperado: 400 | Obtenido: **400** | OK

```json
{"mensaje":"El propietario indicado no existe"}
```

## 19. Crear mascota con nombre repetido para el mismo propietario

- Peticion: `POST /api/mascotas`
- Cuerpo: `{"nombre":"Toby","especie":"Perro","pesoKg":8,"fechaNacimiento":"2022-01-10","activa":true,"propietarioId":4}`
- Esperado: 409 | Obtenido: **409** | OK

```json
{"mensaje":"El propietario ya tiene una mascota con ese nombre"}
```

## 20. Crear mascota con peso invalido

- Peticion: `POST /api/mascotas`
- Cuerpo: `{"nombre":"Pesado","especie":"Perro","pesoKg":-5,"fechaNacimiento":"2022-01-10","activa":true,"propietarioId":4}`
- Esperado: 400 | Obtenido: **400** | OK

```json
{"type":"https://tools.ietf.org/html/rfc9110#section-15.5.1","title":"One or more validation errors occurred.","status":400,"errors":{"PesoKg":["The field PesoKg must be between 0.01 and 200."]},"traceId":"00-17f53e6fcdbe9bf121f36852261a5696-cd3ef5dc73516875-00"}
```

## 21. Actualizar mascota

- Peticion: `PUT /api/mascotas/5`
- Cuerpo: `{"id":5,"nombre":"Toby","especie":"Perro","raza":"Beagle","pesoKg":11.2,"fechaNacimiento":"2022-01-10","activa":true,"propietarioId":4}`
- Esperado: 204 | Obtenido: **204** | OK

```json

```

## 22. Actualizar mascota con id distinto en ruta y cuerpo

- Peticion: `PUT /api/mascotas/5`
- Cuerpo: `{"id":9999,"nombre":"Toby","especie":"Perro","pesoKg":11,"fechaNacimiento":"2022-01-10","activa":true,"propietarioId":4}`
- Esperado: 400 | Obtenido: **400** | OK

```json
{"mensaje":"El id de la ruta no coincide con el del cuerpo"}
```

## 23. Actualizar mascota inexistente

- Peticion: `PUT /api/mascotas/9999`
- Cuerpo: `{"id":9999,"nombre":"Toby","especie":"Perro","pesoKg":11,"fechaNacimiento":"2022-01-10","activa":true,"propietarioId":4}`
- Esperado: 404 | Obtenido: **404** | OK

```json
{"type":"https://tools.ietf.org/html/rfc9110#section-15.5.5","title":"Not Found","status":404,"traceId":"00-8cf80d5773124a054e21619f61e6b8c1-8c0767355d360b20-00"}
```

## 24. Eliminar propietario que tiene mascotas

- Peticion: `DELETE /api/propietarios/4`
- Esperado: 409 | Obtenido: **409** | OK

```json
{"mensaje":"No se puede eliminar un propietario que tiene mascotas registradas"}
```

## 25. Eliminar mascota

- Peticion: `DELETE /api/mascotas/5`
- Esperado: 204 | Obtenido: **204** | OK

```json

```

## 26. Eliminar mascota ya eliminada

- Peticion: `DELETE /api/mascotas/5`
- Esperado: 404 | Obtenido: **404** | OK

```json
{"type":"https://tools.ietf.org/html/rfc9110#section-15.5.5","title":"Not Found","status":404,"traceId":"00-2708ffa5c300e237984a133e8f19524f-390dc313b69e811c-00"}
```

## 27. Eliminar propietario sin mascotas

- Peticion: `DELETE /api/propietarios/4`
- Esperado: 204 | Obtenido: **204** | OK

```json

```

## 28. Eliminar propietario ya eliminado

- Peticion: `DELETE /api/propietarios/4`
- Esperado: 404 | Obtenido: **404** | OK

```json
{"type":"https://tools.ietf.org/html/rfc9110#section-15.5.5","title":"Not Found","status":404,"traceId":"00-27b362d4a9b5147b5950a1b561d6d399-95ed5edefbc50afd-00"}
```

