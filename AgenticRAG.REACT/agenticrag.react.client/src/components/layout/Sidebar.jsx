import React from 'react';
import { NavLink } from 'react-router-dom'; // Only need NavLink here!
import styles from './Sidebar.module.css';

export default function Sidebar({ isOpen, onClose }) {
    return (
        <>
            {isOpen && <div className={styles.sidebarOverlay} onClick={onClose} />}

            <aside className={`${styles.sidebar} ${isOpen ? styles.sidebarOpen : ''}`}>
                <div className={styles.logoArea}>
                    <div className={styles.logoIcon}>🍇</div>
                    <span className={styles.logoText}>BERRY</span>
                </div>

                <nav className={styles.navGroup}>
                    <div className={styles.navSectionTitle}>Dashboard</div>
                    <NavLink
                        to="/"
                        className={({ isActive }) => `${styles.navItem} ${isActive ? styles.active : ''}`}
                    >
                        Default
                    </NavLink>
                    <NavLink
                        to="/analytics"
                        className={({ isActive }) => `${styles.navItem} ${isActive ? styles.active : ''}`}
                    >
                        Analytics
                    </NavLink>

                    <div className={styles.navSectionTitle}>Pages</div>
                    <NavLink
                        to="/users"
                        className={({ isActive }) => `${styles.navItem} ${isActive ? styles.active : ''}`}
                    >
                        Users
                    </NavLink>
                    <NavLink
                        to="/customer"
                        className={({ isActive }) => `${styles.navItem} ${isActive ? styles.active : ''}`}
                    >
                        Customer
                    </NavLink>
                    <NavLink
                        to="/products"
                        className={({ isActive }) => `${styles.navItem} ${isActive ? styles.active : ''}`}
                    >
                        Products
                    </NavLink>
                    <NavLink
                        to="/orders"
                        className={({ isActive }) => `${styles.navItem} ${isActive ? styles.active : ''}`}
                    >
                        Orders
                    </NavLink>

                    <div className={styles.navSectionTitle}>Authentication</div>
                    <NavLink
                        to="/login"
                        className={({ isActive }) => `${styles.navItem} ${isActive ? styles.active : ''}`}
                    >
                        Login
                    </NavLink>
                    <NavLink
                        to="/register"
                        className={({ isActive }) => `${styles.navItem} ${isActive ? styles.active : ''}`}
                    >
                        Register
                    </NavLink>
                </nav>
            </aside>
        </>
    );
}