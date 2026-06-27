import React, { useState } from 'react';
import { Outlet } from 'react-router-dom'; 
import Sidebar from './Sidebar';
import Topbar from './Topbar';
import styles from './Layout.module.css';

export default function MainLayout({ children }) {
    const [sidebarOpen, setSidebarOpen] = useState(false);

    return (
        <div className={styles.layoutContainer}>
            {/* Structural Sidebar */}
            <Sidebar isOpen={sidebarOpen} onClose={() => setSidebarOpen(false)} />

            <div className={styles.mainContent}>
                {/* Structural Top Header Bar */}
                <Topbar onMenuToggle={() => setSidebarOpen(!sidebarOpen)} />

                {/* Dynamic Inner Sub-Page Content Container */}
                <div className={styles.pageBody}>
                    <Outlet />
                </div>
            </div>
        </div>
    );
}