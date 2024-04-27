import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import './App.css';
import Login from './components/login.jsx';
import HomePage from './components/home.jsx';
import AdminPage from './components/admin.jsx';
import Register from './components/register.jsx';
import RegisterAdmin from './components/registerAdmin.jsx';
function App() {
    return (
        <Router>
            <Routes>
                <Route path="/home" element={<HomePage />} />
                <Route path="/admin" element={<AdminPage />} />
                <Route path="/" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route path="/registerAdmin" element={<RegisterAdmin />} />
            </Routes>
        </Router>
    );
}

export default App;