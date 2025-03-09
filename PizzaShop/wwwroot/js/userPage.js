
const toggleSidebar = () => {
  const sidebar = document.getElementById("sidebar");
  const overlay = document.getElementById("overlay");
  sidebar.classList.toggle("active");
  overlay.classList.toggle("active");
};

// document.getElementById("overlay").addEventListener("click", toggleSidebar);


document.getElementById('sidebarToggle').addEventListener('click', function () {
  var sidebar = document.getElementById('sidebar');
  var content = document.getElementById('content');
  var overlay = document.getElementById('overlay');

  // Toggle sidebar visibility
  sidebar.classList.toggle('show');
  overlay.classList.toggle('show');

  // Adjust content margin for sidebar visibility
  if (window.innerWidth <= 1200) {
    if (sidebar.classList.contains('show')) {
      content.style.marginLeft = "0"; // Sidebar is visible
    } else {
      content.style.marginLeft = "0"; // Sidebar is hidden
    }
  }
});

// Overlay click to hide sidebar
// document.getElementById('overlay').addEventListener('click', function () {
//   document.getElementById('sidebar').classList.remove('show');
//   document.getElementById('content').style.marginLeft = "0"; // Full width
//   document.getElementById('overlay').classList.remove('show');
// });

// Adjust layout on window resize
window.addEventListener('resize', function () {
  var sidebar = document.getElementById('sidebar');
  var content = document.getElementById('content');

  if (window.innerWidth > 1200) {
    sidebar.classList.remove('show'); // Ensure sidebar is visible
    content.style.marginLeft = "300px"; // Default margin for larger screens
  } else {
    content.style.marginLeft = "0"; // Full width on small screens
  }
});

// Get all sidebar links
const sidebarLinks = document.querySelectorAll('.sidebar ul li a');

// Function to set active link
function setActiveLink() {
  const activeLink = localStorage.getItem('activeLink');
  if (activeLink) {
    sidebarLinks.forEach(link => {
      if (link.getAttribute('href') === activeLink) {
        link.classList.add('active');
      }
    });
  }
}

// Add click event listener to each link
sidebarLinks.forEach(link => {
  link.addEventListener('click', function () {
    // Remove active class from all links
    sidebarLinks.forEach(item => item.classList.remove('active'));
    // Add active class to the clicked link
    this.classList.add('active');
    // Store the active link in local storage
    localStorage.setItem('activeLink', this.getAttribute('href'));
  });
});
// Set the active link on page load
setActiveLink();