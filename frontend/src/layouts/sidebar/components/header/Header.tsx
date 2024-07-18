import { Button } from "@/components/ui/button/Button"
import { cn } from "@/lib/utils"
import { BsPerson } from "react-icons/bs"
import { IoSettingsOutline } from "react-icons/io5"
import styles from "./Header.module.scss"

type SidebarHeaderProps = {
  onShortClick: () => void
  isShort?: boolean
}

const SidebarHeader = ({onShortClick, isShort}: SidebarHeaderProps) => {
  return (
    <div className={cn(styles.sidebar__header, isShort && styles["sidebar__header--short"])}>
        <Button variant="icon" onClick={onShortClick} className={cn(isShort && "rotate-180")}>
            <IoSettingsOutline />
        </Button>
        <span className={styles.sidebar__title}>
            DexHelper
        </span>
        <Button variant="icon">
            <BsPerson />
        </Button>
    </div>
  )
}

export default SidebarHeader
