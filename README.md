
# TOCABOLAS DSA

Este es un juego estilo *Suika Game* desarrollado en Unity por el grupo 2 de DSA, donde el jugador combina bolas iguales para formar bolas más grandes y conseguir la máxima puntuación posible. El juego cuenta con un sistema de inventario, ítems especiales (bomba, oro, borrar bola), y guardado de estadísticas a través de una API backend.

## Características 🎮
- Generación aleatoria de bolas.
- Colisiones físicas y combinación de bolas del mismo tipo.
- Ítems especiales: bombas, oro (duplica dinero) y eliminación de bolas.
- Sistema de puntuación y dinero acumulado.
- Inventario sincronizado con servidor backend.
- Interfaz limpia y adaptada para móvil.

## Cómo jugar
1. Haz clic/tap para dejar caer una bola.
2. Combina bolas iguales para obtener una más grande.
3. Usa objetos especiales cuando los tengas disponibles.
4. Si las bolas se apilan demasiado alto, ¡pierdes!

## Requisitos
- Unity 2022 o superior.
- Conexión a internet para sincronizar con el backend.
- Backend Java corriendo con la misma API definida.

## API esperada
El juego se comunica con un backend REST usando JWT:
- `/user/stats` → Obtener dinero y récord.
- `/user/inventory` → Obtener inventario.
- `/user/score` → Enviar nueva puntuación.
- `/user/money` → Añadir dinero acumulado.
- `/inventory/use/{id}` → Consumir objeto.

## Créditos
Desarrollado por Laura, Victor, Jan y Omar.
