"use strict"        

// Connect to the SignalR hub
var connection = new signalR.HubConnectionBuilder().withUrl("/appointmentHub").build();

connection.start();
connection.on("ReceiveSlotUpdate", function (doctorEmail, date, slot, isBooked) {

       const email = document.getElementById("email").value;
       const appointmentDate = document.getElementById("appointmentDate").value;

       if (doctorEmail === email && date === appointmentDate) {
             const slotElement = document.querySelector(`[data-slot='${slot}']`);
             if (slotElement) {
                 if (isBooked) {
                       // Add fade-out effect before replacing the content
                       $(slotElement).fadeOut('slow', function () {
                          slotElement.innerHTML = `
                                 <div class="pt-3 rounded-2 border border-dark d-flex flex-column justify-content-center align-items-center text-center text-white" style="background: linear-gradient(90deg, rgb(232, 46, 32) 0%, rgb(51, 7, 4) 100%);">
                                    <i class="bi bi-x-circle fs-5 mb-1"></i>
                                    <p>${slot}</p>
                                </div>
                                 `;
                          $(slotElement).fadeIn('slow'); // Add fade-in effect after content replacement
                       });

                     // Call the function to disable the link
                     disableLink(slotElement);
                        

                 }
            }
       }
});


// Function to disable the link
function disableLink(element) {
    if (element) {
        // Prevent the default action
        element.addEventListener('click', function (event) {
            event.preventDefault();
            event.stopPropagation();
        });

        // Apply the disabled style
        element.classList.add('disabled-link');

        // Optionally, you can remove attributes that might trigger actions
        element.removeAttribute('data-bs-toggle');
        element.removeAttribute('data-bs-target');
    }
}