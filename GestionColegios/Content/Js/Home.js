let toggleBtn = document.getElementById('toggle-btn');
let body = document.body;
let darkMode = localStorage.getItem('dark-mode');

const enableDarkMode = () => {
    toggleBtn.classList.replace('fa-sun', 'fa-moon');
    body.classList.add('dark');
    localStorage.setItem('dark-mode', 'enabled');
}

const disableDarkMode = () => {
    toggleBtn.classList.replace('fa-moon', 'fa-sun');
    body.classList.remove('dark');
    localStorage.setItem('dark-mode', 'disabled');
}

if (darkMode === 'enabled') {
    enableDarkMode();
}

toggleBtn.onclick = (e) => {
    darkMode = localStorage.getItem('dark-mode');
    if (darkMode === 'disabled') {
        enableDarkMode();
    } else {
        disableDarkMode();
    }
}

let profile = document.querySelector('.header .flex .profile');

document.querySelector('#user-btn').onclick = () => {
    profile.classList.toggle('active');
    search.classList.remove('active');
}

let search = document.querySelector('.header .flex .search-form');

document.querySelector('#search-btn').onclick = () => {
    search.classList.toggle('active');
    profile.classList.remove('active');
}

let sideBar = document.querySelector('.side-bar');

document.querySelector('#menu-btn').onclick = () => {
    sideBar.classList.toggle('active');
    body.classList.toggle('active');
}

document.querySelector('#close-btn').onclick = () => {
    sideBar.classList.remove('active');
    body.classList.remove('active');
}

window.onscroll = () => {
    profile.classList.remove('active');
    search.classList.remove('active');

    if (window.innerWidth < 1200) {
        sideBar.classList.remove('active');
        body.classList.remove('active');
    }
}


//Modal de guardado
document.addEventListener('DOMContentLoaded', function () {
    // Escucha los clics en botones con el ID btn-save
    const saveButtons = document.querySelectorAll('#btn-save');

    saveButtons.forEach((button) => {
        button.addEventListener('click', function (event) {
            event.preventDefault(); // Evita el comportamiento por defecto del formulario

            // Referencia al modal y sus estados
            const modal = new bootstrap.Modal(document.getElementById('saveModal'));
            const loadingState = document.getElementById('loadingState');
            const successState = document.getElementById('successState');

            // Muestra el modal y configura el estado inicial
            modal.show();
            loadingState.classList.remove('d-none');
            successState.classList.add('d-none');

            // Simula un proceso de guardado
            setTimeout(() => {
                // Cambia al estado de éxito
                loadingState.classList.add('d-none');
                successState.classList.remove('d-none');
            }, 2000); // Muestra el spinner durante 3 segundos

            // Cierra el modal automáticamente y envía el formulario (si aplica)
            setTimeout(() => {
                modal.hide();

                // Envía el formulario si el botón está dentro de uno
                const form = button.closest('form');
                if (form) {
                    form.submit();
                }
            }, 4000); // Cierra después de 5 segundos
        });
    });
});



//Modal de eliminado
document.addEventListener('DOMContentLoaded', function () {
    // Escucha los clics en botones con el ID btn-confirm-delete
    const deleteButtons = document.querySelectorAll('#btn-confirm-delete');

    deleteButtons.forEach((button) => {
        button.addEventListener('click', function (event) {
            event.preventDefault(); // Evita el comportamiento por defecto del formulario

            // Referencias a los modales y sus estados
            const deleteSuccessModal = new bootstrap.Modal(document.getElementById('deleteSuccessModal'));
            const loadingState = document.getElementById('loadingState');
            const successState = document.getElementById('successState');
            
            // Obtener los elementos de los modales
            const deleteModalElement = document.getElementById('deleteModal');
            const deleteSuccessModalElement = document.getElementById('deleteSuccessModal');
            
            // Ajuste de z-index para asegurarse de que el modal de éxito esté encima del de eliminación
            deleteModalElement.style.zIndex = '1050'; // Z-index del modal de confirmación
            deleteSuccessModalElement.style.zIndex = '1051'; // Z-index del modal de éxito

            // Cierra el modal de confirmación de eliminación
            const deleteModal = new bootstrap.Modal(deleteModalElement);
            deleteModal.hide();

            // Muestra el modal de borrado exitoso
            deleteSuccessModal.show();
            loadingState.classList.remove('d-none');
            successState.classList.add('d-none');

            // Simula un proceso de eliminación
            setTimeout(() => {
                // Cambia al estado de éxito de eliminación
                loadingState.classList.add('d-none');
                successState.classList.remove('d-none');
            }, 2000); // Muestra el spinner durante 2 segundos

            // Cierra el modal de borrado exitoso automáticamente después de 10 segundos
            setTimeout(() => {
                deleteSuccessModal.hide();

                // Enviar el formulario después del proceso
                const form = button.closest('form');
                if (form) {
                    form.submit(); // Envía el formulario después del proceso
                }
            }, 4000); // Cierra después de 10 segundos
        });
    });
});
//Modal de Extender sesion
document.addEventListener('DOMContentLoaded', function () {
    const sessionTimeoutWarning = 50 * 60 * 1000; // 10 minutos antes de cerrar sesion
    const sessionModal = new bootstrap.Modal(document.getElementById('extendSessionModal')); // ID del modal
    const sessionTimerElement = document.getElementById('sessionTimer'); // Elemento del temporizador
    const extendSessionButton = document.getElementById('btn-extend-session'); // Botón para extender sesión
    const logoutButton = document.getElementById('btn-logout'); // Botón para cerrar sesión
    let timeoutWarningShown = false; // Evita mostrar múltiples advertencias
    let sessionExpiryTime; // Temporizador para la sesión
    let timer; // Tiempo restante en segundos
    let timerInterval; // Intervalo del temporizador dinámico

    // Función para mostrar el modal de advertencia
    function showSessionExpiryWarning() {
        if (!timeoutWarningShown) {
            timeoutWarningShown = true;
            timer = 10 * 60; // 10 minutos antes de cerrar la sesión automáticamente
            sessionTimerElement.textContent = formatTime(timer); // Inicializa el temporizador en el modal
            sessionModal.show(); // Muestra el modal

            // Temporizador dinámico para el modal
            timerInterval = setInterval(() => {
                timer--;
                sessionTimerElement.textContent = formatTime(timer);

                if (timer <= 0) {
                    clearInterval(timerInterval);
                    sessionModal.hide();
                    // Redirige al usuario al cierre de sesión
                    logoutUser();
                }
            }, 1000);
        }
    }

    // Función para formatear el tiempo en MM:SS
    function formatTime(seconds) {
        const minutes = Math.floor(seconds / 60);
        const remainingSeconds = seconds % 60;
        return `${minutes}:${remainingSeconds.toString().padStart(2, "0")}`;
    }

    // Configurar el temporizador para mostrar advertencia antes de que la sesión expire
    sessionExpiryTime = setTimeout(function () {
        showSessionExpiryWarning();
    }, sessionTimeoutWarning);

    function resetSessionTimer() {
        clearTimeout(sessionExpiryTime); // Limpiar el temporizador anterior
        clearInterval(timerInterval); // Detener el temporizador dinámico del modal si está activo
        timeoutWarningShown = false; // Restablecer el aviso
        sessionExpiryTime = setTimeout(function () {
            showSessionExpiryWarning();
        }, sessionTimeoutWarning);
    }

    // Función para cerrar sesión
    function logoutUser() {
        const csrfToken = document.querySelector('input[name="__RequestVerificationToken"]').value;

        fetch('/Account/LogOff', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
                'X-CSRF-TOKEN': csrfToken // Incluir el token CSRF
            },
            body: `__RequestVerificationToken=${encodeURIComponent(csrfToken)}`
        })
            .then(response => {
                if (response.ok) {
                    window.location.href = '/Home/Index'; // O redirige a la página de inicio
                } else {
                    throw new Error('Error al cerrar la sesión.');
                }
            })
            .catch(error => {
                console.error(error);
                alert('Su sesión se cerró. Redirigiendo al Login.');
                setTimeout(() => {
                    window.location.href = '/Account/Login'; // Redirige al login después de mostrar el mensaje
                }, 5000); // Retraso de 5 segundos
            });
    }

    // Botón para extender sesión
    extendSessionButton.addEventListener('click', () => {
        const csrfToken = document.querySelector('input[name="__RequestVerificationToken"]').value;

        fetch('/Account/ExtendSession', { // Cambia la URL por el endpoint de tu método
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
                'X-CSRF-TOKEN': csrfToken // Incluir el token CSRF
            },
            body: `__RequestVerificationToken=${encodeURIComponent(csrfToken)}`
        })
            .then(response => response.json()) // Espera el JSON de la respuesta
            .then(data => {
                if (data.success) {
                    clearInterval(timerInterval); // Detiene el temporizador del modal
                    sessionModal.hide(); // Oculta el modal
                    resetSessionTimer(); // Reinicia el temporizador de la sesión
                    alert(data.message); // Muestra mensaje de éxito
                } else {
                    throw new Error(data.message); // Muestra mensaje de error si la extensión de sesión falla
                }
            })
            .catch(error => {
                console.error(error);
                alert('Su sesión se cerró. Redirigiendo al Login.');
                setTimeout(() => {
                    window.location.href = '/Account/Login'; // Redirige al login después de mostrar el mensaje
                }, 5000); // Retraso de 5 segundos
            });
    });

    // Botón para cerrar sesión manualmente
    logoutButton.addEventListener('click', () => {
        clearInterval(timerInterval); // Detiene el temporizador del modal
        logoutUser(); // Llamar a la función para cerrar sesión
    });
});