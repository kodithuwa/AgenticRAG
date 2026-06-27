import React from 'react';
import styles from './Dashboard.module.css';

export default function Dashboard() {
    return (
            <main className={styles.gridContainer}>
                {/* Top Metric Cards */}
                <div className={`${styles.card} ${styles.purpleCard}`}>
                    <div className={styles.cardHeaderRow}>
                        <div className={styles.cardIconBox}>💰</div>
                        <button className={styles.moreBtn}>•••</button>
                    </div>
                    <div className={styles.cardValue}>$500.00</div>
                    <div className={styles.cardLabel}>Total Earnings</div>
                    <div className={styles.cardSubtext}>+16.24% from last month</div>
                </div>

                <div className={`${styles.card} ${styles.blueCard}`}>
                    <div className={styles.cardHeaderRow}>
                        <div className={styles.cardIconBox}>🛍️</div>
                        <button className={styles.moreBtn}>•••</button>
                    </div>
                    <div className={styles.cardValue}>$961</div>
                    <div className={styles.cardLabel}>Total Orders</div>
                    <div className={styles.cardSubtext}>+12.45% from last month</div>
                </div>

                <div className={`${styles.card} ${styles.tealCard}`}>
                    <div className={styles.cardHeaderRow}>
                        <div className={styles.cardIconBox}>🏪</div>
                        <button className={styles.moreBtn}>•••</button>
                    </div>
                    <div className={styles.cardValue}>$203k</div>
                    <div className={styles.cardLabel}>Total Income</div>
                    <div className={styles.cardSubtext}>+8.65% from last month</div>
                </div>

                {/* Chart Card */}
                <div className={`${styles.card} ${styles.chartCard}`}>
                    <div className={styles.sectionHeader}>
                        <div>
                            <span>Total Growth</span>
                            <h2>$2,324.00</h2>
                        </div>
                        <select className={styles.dropdown}>
                            <option>This Year</option>
                        </select>
                    </div>
                    <div className={styles.chartVisualPlaceholder}>
                        <div className={styles.mockBar} style={{ '--h1': '30px', '--h2': '40px' }}></div>
                        <div className={styles.mockBar} style={{ '--h1': '80px', '--h2': '60px' }}></div>
                        <div className={styles.mockBar} style={{ '--h1': '40px', '--h2': '20px' }}></div>
                        <div className={styles.mockBar} style={{ '--h1': '50px', '--h2': '30px' }}></div>
                        <div className={styles.mockBar} style={{ '--h1': '90px', '--h2': '70px' }}></div>
                    </div>
                    <div className={styles.chartLegend}>
                        <span className={styles.legendItem}><span className={styles.dotPurp}></span> Investment</span>
                        <span className={styles.legendItem}><span className={styles.dotBlue}></span> Profit</span>
                        <span className={styles.legendItem}><span className={styles.dotTeal}></span> Maintenance</span>
                    </div>
                </div>

                {/* Popular Stocks */}
                <div className={`${styles.card} ${styles.stocksCard}`}>
                    <div className={styles.sectionHeader}>
                        <h3>Popular Stocks</h3>
                        <a href="#viewall" className={styles.viewAllLink}>View All</a>
                    </div>
                    <div className={styles.stockList}>
                        <div className={styles.stockRow}>
                            <div>
                                <div className={styles.stockName}>Bajaj Finery</div>
                                <div className={styles.stockProfit}>10% Profit</div>
                            </div>
                            <div className={styles.stockPrice}>$1839.00</div>
                        </div>
                    </div>
                </div>

                {/* Recent Orders Table */}
                <div className={`${styles.card} ${styles.ordersCard}`}>
                    <div className={styles.sectionHeader}>
                        <h3>Recent Orders</h3>
                        <a href="#viewall" className={styles.viewAllLink}>View All</a>
                    </div>
                    <div className={styles.tableResponsiveWrapper}>
                        <table className={styles.table}>
                            <thead>
                                <tr>
                                    <th>Order ID</th>
                                    <th>Customer</th>
                                    <th>Date</th>
                                    <th>Amount</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>#12548</td>
                                    <td>John Doe</td>
                                    <td>12 May, 2024</td>
                                    <td>$1250.00</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>

                {/* Top Customers */}
                <div className={`${styles.card} ${styles.customersCard}`}>
                    <div className={styles.sectionHeader}>
                        <h3>Top Customers</h3>
                        <a href="#viewall" className={styles.viewAllLink}>View All</a>
                    </div>
                    <div className={styles.customerList}>
                        <div className={styles.customerRow}>
                            <div className={styles.customerInfo}>
                                <div className={styles.smallAvatar}>JD</div>
                                <div>
                                    <div className={styles.customerName}>John Doe</div>
                                    <div className={styles.customerEmail}>john@mail.com</div>
                                </div>
                            </div>
                            <div className={styles.customerProgressWrapper}>
                                <span>$12,450.00</span>
                                <div className={styles.progressBar}><div className={styles.progressFill} style={{ width: '85%' }}></div></div>
                            </div>
                        </div>
                    </div>
                </div>

            </main>
    );
}