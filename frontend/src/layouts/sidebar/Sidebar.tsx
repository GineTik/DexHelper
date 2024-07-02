"use client"
import { cn } from "@/lib/utils"
import { useState } from "react"
import styles from "./Sidebar.module.scss"
import SidebarContent from "./components/content/SidebarContent"
import SidebarHeader from "./components/header/Header"

const Sidebar = () => {
  const [isShort, setShort] = useState(false)

  return (
    <div className={cn(styles.sidebar, isShort && styles["sidebar--short"])}>
        <div className={styles.sidebar__container}>
          <SidebarHeader isShort={isShort} onShortClick={() => setShort(o => !o)} />
          <SidebarContent />
        </div>
    </div>
  )
}

export default Sidebar
