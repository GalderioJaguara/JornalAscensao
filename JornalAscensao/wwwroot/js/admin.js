function toggleSideNav() {
    const sideNav = document.querySelector('.sidenav');
    const adminContainer = document.querySelector('.admin-container');
    sideNav.classList.toggle('sidenav-collapsed');
    adminContainer.classList.toggle('container-collapsed')
}