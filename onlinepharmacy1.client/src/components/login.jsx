import { useState } from 'react';
import axios from 'axios';
import CryptoJS from 'crypto-js';

function base64UrlDecode(str) {
    str = str.replace(/-/g, '+').replace(/_/g, '/');
    while (str.length % 4) {
        str += '=';
    }
    return decodeURIComponent(atob(str).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));
}

function jwtDecode(token) {
    var parts = token.split('.');
    if (parts.length !== 3) {
        throw new Error('JWT must have 3 parts');
    }
    var decoded = base64UrlDecode(parts[1]);
    return JSON.parse(decoded);
}

function Login() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [loginStatus, setLoginStatus] = useState('');

    const handleLogin = async (event) => {
        event.preventDefault();

        const formData = new FormData();
        formData.append('email', email);
        formData.append('password', password);

        try {
            const response = await axios.post("http://localhost:5209/api/Account/Login", formData);

            if (response.status === 200) {
                localStorage.setItem('token', response.data.token);
                setLoginStatus('Login successful');

                // Decode the token and check the user's role
                const decodedToken = jwtDecode(response.data.token);
                const userRole = decodedToken.role;

                // Redirect the user based on their role
                if (userRole === 'Admin') {
                    window.location.href = '/admin'; // Redirect to admin page
                } else {
                    window.location.href = '/home'; // Redirect to home page
                }
            } else {
                throw new Error('Login failed');
            }
        } catch (error) {
            setLoginStatus('Login failed');
            console.error('Error:', error);
        }
    }


    return (
        <div>
            <h1>Login</h1>
            <form onSubmit={handleLogin}>
                <label>
                    Email:
                    <input type="text" value={email} onChange={e => setEmail(e.target.value)} />
                </label>
                <label>
                    Password:
                    <input type="password" value={password} onChange={e => setPassword(e.target.value)} />
                </label>
                <input type="submit" value="Submit" />
            </form>
            <button onClick={() => window.location.href = '/register'}>Register</button>
            <button onClick={() => window.location.href = '/registerAdmin'}>Register for Admin</button>

            <p>{loginStatus}</p>
        </div>

    );
}

export default Login;
