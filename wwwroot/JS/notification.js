window.ShowToastr = function (type, message) {
    if (type === "success") {
        toastr.success(message);
    }
    if (type == "error") {
        toastr.error(message);
    }
}
function ShowConfirmationModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('bsConfirmationModal')).show();
}

function HideConfirmationModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('bsConfirmationModal')).hide();
}
function ShowAssignTicketFormModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('AssignTicket')).show();
}

function HideAssignTicketFormModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('AssignTicket')).hide();
}
function ShowCloseTicketFormModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('CloseTicket')).show();
}

function HideCloseTicketFormModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('CloseTicket')).hide();
} function ShowAddSolutionFormModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('AddSolution')).show();
}

function HideAddSolutionFormModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('AddSolution')).hide();
} function ShowAddTaskFormModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('AddTask')).show();
}

function HideAddTaskFormModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('AddTask')).hide();
} function ShowVerificationModalFormModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('VerificationModal')).show();
}

function HideVerificationModalFormModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('VerificationModal')).hide();
} function ShowResetPasswordFormModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('ResetPassword')).show();
}

function HideResetPasswordFormModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('ResetPassword')).hide();
}
function setupFileUpload() {
    const fileInput = document.getElementById('fileUpload');
    const fileNameDisplay = document.getElementById('fileName');

    if (fileInput) {
        fileInput.addEventListener('change', function (e) {
            if (this.files && this.files.length > 0) {
                fileNameDisplay.textContent = this.files[0].name;
            } else {
                fileNameDisplay.textContent = '';
            }
        });
    }
}
window.setupFileUpload = setupFileUpload;



window.floatingNotification = {
    init: function (dotNetHelper) {
        // Close notifications when clicking outside
        document.addEventListener('click', function (e) {
            const bellContainer = document.querySelector('.floating-notification-bell');
            if (!bellContainer.contains(e.target)) {
                dotNetHelper.invokeMethodAsync('CloseNotifications');
            }
        });

        // Optional: Add keyboard escape support
        document.addEventListener('keydown', function (e) {
            if (e.key === 'Escape') {
                dotNetHelper.invokeMethodAsync('CloseNotifications');
            }
        });
    }
};
// Scroll to bottom of chat messages
function scrollToBottom(element) {
    element.scrollTop = element.scrollHeight;
}

// Download file helper
function downloadFile(dataUrl, fileName) {
    const link = document.createElement('a');
    link.href = dataUrl;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}



// Image preview handler (optional)
function setupImagePreview(inputElement, previewElement) {
    inputElement.addEventListener('change', function (e) {
        const file = e.target.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function (event) {
                previewElement.src = event.target.result;
                previewElement.style.display = 'block';
            }
            reader.readAsDataURL(file);
        }
    });
}

// Keyboard shortcuts
function setupChatShortcuts(textareaElement, sendButton) {
    textareaElement.addEventListener('keydown', function (e) {
        if (e.key === 'Enter' && !e.shiftKey) {
            e.preventDefault();
            sendButton.click();
        }
    });
}

// Initialize everything when DOM is loaded
document.addEventListener('DOMContentLoaded', function () {
    // Get elements - you may need to adjust these selectors
    const chatContainer = document.querySelector('.chat-messages');
    const fileInput = document.querySelector('.file-upload input');
    const imagePreview = document.querySelector('.image-preview img');
    const messageTextarea = document.querySelector('.chat-input-area textarea');
    const sendButton = document.querySelector('.btn-send');

    // Set up functionality
    if (chatContainer) scrollToBottom(chatContainer);
    if (fileInput && imagePreview) setupImagePreview(fileInput, imagePreview);
    if (messageTextarea && sendButton) setupChatShortcuts(messageTextarea, sendButton);

    // Fullscreen image click handlers
    document.querySelectorAll('.message-image, .image-preview img').forEach(img => {
        img.addEventListener('click', function () {
            const fullscreenOverlay = document.createElement('div');
            fullscreenOverlay.className = 'fullscreen-image-overlay';
            fullscreenOverlay.innerHTML = `
                <div class="fullscreen-image-container">
                    <img src="${this.src}" class="fullscreen-image" />
                    <button class="close-fullscreen">
                        <i class="fas fa-times"></i>
                    </button>
                    <button class="download-image">
                        <i class="fas fa-download"></i>
                    </button>
                </div>
            `;

            document.body.appendChild(fullscreenOverlay);

            // Close button
            fullscreenOverlay.querySelector('.close-fullscreen').addEventListener('click', function () {
                document.body.removeChild(fullscreenOverlay);
            });

            // Download button
            fullscreenOverlay.querySelector('.download-image').addEventListener('click', function () {
                const fileName = `ticket-chat-image-${new Date().getTime()}.jpg`;
                downloadFile(img.src, fileName);
            });

            // Click outside to close
            fullscreenOverlay.addEventListener('click', function (e) {
                if (e.target === this) {
                    document.body.removeChild(fullscreenOverlay);
                }
            });
        });
    });
});

// Mobile menu toggle
const mobileMenuBtn = document.getElementById('mobile-menu-btn');
const mainNav = document.getElementById('main-nav');

mobileMenuBtn.addEventListener('click', function () {
    this.classList.toggle('active');
    mainNav.classList.toggle('active');
});

// Show login form


// Create floating elements for hero section
const floatingElements = document.getElementById('floating-elements');
for (let i = 0; i < 15; i++) {
    const element = document.createElement('div');
    element.classList.add('floating-element');

    // Random size between 5px and 20px
    const size = Math.random() * 15 + 5;
    element.style.width = `${size}px`;
    element.style.height = `${size}px`;

    // Random position
    element.style.left = `${Math.random() * 100}%`;
    element.style.top = `${Math.random() * 100}%`;

    // Random animation duration between 10s and 20s
    element.style.animationDuration = `${Math.random() * 10 + 10}s`;

    // Random delay
    element.style.animationDelay = `${Math.random() * 5}s`;

    floatingElements.appendChild(element);
}

// Create particles for stats section
const particles1 = document.getElementById('particles-1');
for (let i = 0; i < 30; i++) {
    const particle = document.createElement('div');
    particle.classList.add('particle');

    // Random size between 2px and 5px
    const size = Math.random() * 3 + 2;
    particle.style.width = `${size}px`;
    particle.style.height = `${size}px`;

    // Random position
    particle.style.left = `${Math.random() * 100}%`;
    particle.style.top = `${Math.random() * 100}%`;

    // Random animation duration between 10s and 20s
    particle.style.animationDuration = `${Math.random() * 10 + 10}s`;

    // Random delay
    particle.style.animationDelay = `${Math.random() * 5}s`;

    // Random opacity
    particle.style.opacity = Math.random() * 0.5 + 0.1;

    particles1.appendChild(particle);
}

// Animate elements when they come into view
const animateOnScroll = () => {
    const elements = document.querySelectorAll('.animate-fadeInUp, .animate__animated');

    elements.forEach(element => {
        const elementPosition = element.getBoundingClientRect().top;
        const screenPosition = window.innerHeight / 1.2;

        if (elementPosition < screenPosition) {
            if (element.classList.contains('animate-fadeInUp')) {
                element.style.opacity = '1';
                element.style.transform = 'translateY(0)';
            } else {
                const animationClass = element.classList[1];
                element.classList.add(animationClass);
            }
        }
    });
};

window.addEventListener('scroll', animateOnScroll);
window.addEventListener('load', animateOnScroll);


function downloadFile(fileName, base64Data) {
    const link = document.createElement('a');
    link.href = 'data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,' + base64Data;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

