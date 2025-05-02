<template>
    <nav>
        <ul class="navbar">
            <li><router-link to="/">All orders</router-link></li>
            <li><router-link to="/user-orders">User orders</router-link></li>
            <li><router-link to="/user-account">Account</router-link></li>
        </ul>
    </nav>
</template>

<script>
export default {
  name: 'NavbarComponent'
}
</script>

<style>
.navbar {
    list-style-type: none;
    margin: 0;
    padding: 0;
    display: flex;
    background-color: #333;
}
.navbar li {
    padding: 14px 20px;
}
.navbar li a {
    color: white;
    text-decoration: none;
}
.navbar li a:hover {
    background-color: #111;
}
.navbar li a.active {
    background-color: #4CAF50;
    color: white;
}
.navbar li a:visited {
    color: white;
}
.navbar li a:focus {
    outline: none;
}
</style>