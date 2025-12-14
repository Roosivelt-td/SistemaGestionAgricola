# Frontend Agrícola

Aplicación de gestión agrícola con sistema de autenticación completo.

## Instalación

1. Clona el repositorio
2. Navega al directorio del frontend: `cd frontend-agricola`
3. Instala las dependencias: `npm install`

## Variables de Entorno

Crea un archivo `.env` en el directorio raíz con las siguientes variables:

```
VITE_API_BASE_URL=http://localhost:3000/api
VITE_GOOGLE_CLIENT_ID=tu_client_id_de_google_aqui
```

## Ejecución

Ejecuta el siguiente comando para iniciar la aplicación en modo desarrollo:

```bash
npm run dev
```

La aplicación estará disponible en `http://localhost:5173`

## Paquetes utilizados

- React 18 con TypeScript
- react-router-dom para enrutamiento
- axios para peticiones HTTP
- @react-oauth/google para autenticación con Google
- Context API para manejo de estado global
- Validaciones personalizadas para formularios

## Características

- Sistema de autenticación completo (login/registro)
- Validaciones de formularios en tiempo real
- Autenticación con Google
- Manejo de tokens JWT
- Interceptores de Axios para manejo de autorización
- Rutas protegidas
- Sistema de temas claro/oscuro