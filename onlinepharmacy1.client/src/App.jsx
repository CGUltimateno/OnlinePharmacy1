import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import './App.css';
import Login from './components/login.jsx';
import HomePage from './components/home.jsx';
import AdminPage from "./components/admin.jsx";

import Register from './components/register.jsx';
import RegisterAdmin from './components/registerAdmin.jsx';

function App() {
    const [userRole, setUserRole] = useState('');

    useEffect(() => {
        // You can perform any initial setup or authentication checks here
        // For example, you might want to make an API call to check the user's role
        
        // For now, let's just set the user role to a default value
        setUserRole('user');
    }, []);

    return (
        <Router>
            <Routes>
                <Route path="/home" element={<HomePage />} />
                 <Route path="/admin" element={userRole === 'admin' ? <AdminPage /> : <Navigate to="/home" />} />

                <Route path="/" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route path="/registerAdmin" element={<RegisterAdmin />} />
            </Routes>
        </Router>
    );
}

export default App;
