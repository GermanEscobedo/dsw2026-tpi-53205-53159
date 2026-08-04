# Trabajo Práctico Integrador - Desarrollo de Software 2026

## Integrantes

- Escobedo, Lautaro German — Legajo 53205
- Castro, Facundo Leonel — Legajo 53159

## Descripción del proyecto

Backend desarrollado para la gestión de turnos médicos, con control de acceso basado en roles (Administrador, Doctor, Paciente).

## Configuración y ejecución local

### 1. Clonar el repositorio

git clone https://github.com/GermanEscobedo/dsw2026-tpi-53205-53159.git
cd dsw2026-tpi-53205-53159

### 2. Verificar conexion SQL

Verificar la conexión a SQL Server en appsettings.json o en los User Secrets del proyecto Dsw2026Tpi.Api:


{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TurnosMedicosDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}


### 3. Aplicar las migraciones de la base de datos

Migración de los esquemas de identidad y usuarios:


dotnet ef database update --context AuthenticationDbContext --project Dsw2026Tpi.Data --startup-project Dsw2026Tpi.Api


Migración de los esquemas de dominio (médicos, pacientes, turnos, historias clínicas):


dotnet ef database update --context Dsw2026TpiDbContext --project Dsw2026Tpi.Data --startup-project Dsw2026Tpi.Api


### 4. Ejecutar el proyecto

Desde el IDE o por consola:


dotnet run --project Dsw2026Tpi.Api


### 5. Exposición remota mediante Ngrok (ya autenticado)


ngrok http 5278


Luego acceder a /swagger a continuación del link generado por ngrok.

## Endpoints y modo de uso

Para acceder a los endpoints protegidos por roles, se debe enviar el token JWT en la cabecera HTTP Authorization: Bearer TOKEN.

### 1. Autenticación y autorización — /api/auth

- POST /api/auth/register: registro inicial de usuarios administradores (permite crear la cuenta requerida para pruebas).
  - Solicitud: { "email": "admin@test.com", "password": "Admin123!" }

- POST /api/auth/admin/login: autenticación de usuarios administradores.
  - Solicitud: { "email": "admin@test.com", "password": "Admin123!" }
  
- POST /api/auth/patient/login: autenticación y auto-registro implícito de pacientes.
  - Solicitud: { "email": "paciente@test.com", "dni": "40123456" }
  

### 2. Especialidades médicas — /api/specialities

- POST /api/specialities: alta de una nueva especialidad.
  - Solicitud: { "name": "Cardiología", "description": "Atención especializada del corazón" }
- GET /api/specialities: listado general de especialidades.
- GET /api/specialities/{id}: obtiene una especialidad específica por su GUID.
- PUT /api/specialities/{id}: modifica una especialidad existente.
  - Solicitud: { "name": "Cardiología Infantil", "description": "Atención especializada en niños" }
- DELETE /api/specialities/{id}: da de baja una especialidad por su GUID.

### 3. Médicos — /api/doctors

- POST /api/doctors: registra un nuevo médico asociado a una especialidad existente.
  - Solicitud: { "firstName": "Alejandro", "lastName": "Martínez", "email": "dr.martinez@hospital.com", "licenseNumber": "MP-99201", "specialityId": "GUID" }
- GET /api/doctors: listado general de médicos registrados.
- GET /api/doctors/{id}: obtiene los datos de un médico específico por su GUID.
- PUT /api/doctors/{id}: modifica los datos personales o profesionales de un médico.
  - Solicitud: { "firstName": "Alejandro", "lastName": "Martínez", "email": "dr.martinez@hospital.com", "licenseNumber": "MP-99201", "specialityId": "GUID" }
- DELETE /api/doctors/{id}: da de baja a un médico por su GUID.

### 4. Disponibilidades horarias — /api/doctor-availabilities

- POST /api/doctor-availabilities: configura las franjas horarias de atención semanal de un médico.
  - Solicitud: { "doctorId": "GUID", "dayOfWeek": 1, "startTime": "08:00:00", "endTime": "12:00:00" }
- GET /api/doctor-availabilities: lista todas las disponibilidades horarias configuradas.
- GET /api/doctor-availabilities/doctor/{doctorId}: consulta la disponibilidad asignada a un médico específico por su GUID.
- GET /api/doctor-availabilities/{id}: obtiene los detalles de una disponibilidad por su GUID.
- PUT /api/doctor-availabilities/{id}: modifica o reescribe una disponibilidad horaria existente.
  - Solicitud: { "doctorId": "GUID", "dayOfWeek": 1, "startTime": "09:00:00", "endTime": "13:00:00" }
- DELETE /api/doctor-availabilities/{id}: da de baja una disponibilidad horaria por su GUID.

### 5. Pacientes — /api/patients

- POST /api/patients: registra un nuevo paciente manualmente.
  - Solicitud: { "firstName": "Carlos", "lastName": "Gómez", "email": "carlos@gmail.com", "dni": "40123456", "phoneNumber": "3815551234" }
- GET /api/patients: listado general de pacientes registrados.
- GET /api/patients/{id}: obtiene los datos personales de un paciente específico por su GUID.
- PUT /api/patients/{id}: actualiza los datos personales o de contacto de un paciente.
  - Solicitud: { "firstName": "Carlos", "lastName": "Gómez", "email": "carlos.gomez@gmail.com", "dni": "40123456", "phoneNumber": "3815559999" }
- DELETE /api/patients/{id}: da de baja a un paciente por su GUID.

### 6. Turnos y citas médicas — /api/appointments

- POST /api/appointments: reserva un nuevo turno médico.
  - Solicitud: { "patientId": "GUID", "doctorId": "GUID", "appointmentDateTime": "2026-08-17T09:00:00", "reason": "Control cardiológico" }
  - Validación: verifica disponibilidad del médico, solapamientos y rechaza reservas en días feriados (400 Bad Request).
- GET /api/appointments/search: búsqueda avanzada paginada.
- GET /api/appointments: lista la totalidad de los turnos registrados.
- GET /api/appointments/{id}: obtiene los datos de un turno por su GUID.
- GET /api/appointments/patient/{patientId}: lista los turnos asociados a un paciente específico.
- GET /api/appointments/doctor/{doctorId}: lista los turnos asignados a un médico específico.
- PUT /api/appointments/{id}: modifica la fecha, hora o motivo de un turno existente.
  - Solicitud: { "patientId": "GUID", "doctorId": "GUID", "appointmentDateTime": "2026-08-18T10:00:00", "reason": "Reprogramación de consulta" }
- PUT /api/appointments/{id}/cancel: cancela un turno médico existente.
- DELETE /api/appointments/{id}: cancela o da de baja un turno por su GUID.

### 7. Historias clínicas — /api/medical-records

- POST /api/medical-records: registra una nueva consulta o historia clínica.
  - Solicitud: { "patientId": "GUID", "doctorId": "GUID", "appointmentId": "GUID_OPCIONAL", "consultationDate": "2026-08-04T15:00:00", "diagnosis": "Evolución favorable", "treatment": "Sin medicación", "observations": "Sin particularidades" }
- GET /api/medical-records: listado general de historias clínicas registradas.
- GET /api/medical-records/{id}: obtiene los datos de una historia clínica específica por su GUID.
- GET /api/medical-records/patient/{patientId}: consulta el historial médico completo de un paciente por su GUID.
- GET /api/medical-records/doctor/{doctorId}: consulta las historias clínicas redactadas por un médico por su GUID.
- PUT /api/medical-records/{id}: actualiza el diagnóstico, tratamiento u observaciones de una historia clínica.
  - Solicitud: { "diagnosis": "Diagnóstico actualizado", "treatment": "Tratamiento indicado", "observations": "Observaciones adicionales" }
- DELETE /api/medical-records/{id}: elimina una historia clínica por su GUID.
