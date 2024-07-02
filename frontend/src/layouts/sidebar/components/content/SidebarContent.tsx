import { Button } from "@/components/ui/button/Button"
import { ScrollArea } from "@/components/ui/scroll-area/ScrollArea"
import { cn } from "@/lib/utils"
import { CgMoreAlt } from "react-icons/cg"
import TokenRow from "../token-row/TokenRow"
import styles from "./SidebarContent.module.scss"

const SidebarContent = () => {
  return (
    <ScrollArea className={styles.sidebar__content} scrollClassNames={styles["sidebar__content-scroll"]}>
        <table className={cn(styles.sidebar__table, styles.table)}>
            <tbody>
            <tr className={styles.table__header}>
                <th></th>
                <th>Name</th>
                <th>MCap</th>
                <th>Created</th>
                <th></th>
            </tr>
            <TokenRow />
            <TokenRow />
            <TokenRow />
            <TokenRow />
            <TokenRow />
            <TokenRow />
            <TokenRow />
            <TokenRow />
            <TokenRow />
            <TokenRow />
            <TokenRow />
            <TokenRow />
            <TokenRow />
            <TokenRow />
            <TokenRow />
            <TokenRow />
            <TokenRow />
            <TokenRow />
            <TokenRow />
            <TokenRow />
            <TokenRow />
            <TokenRow />
            <TokenRow />
            <TokenRow />
            </tbody>
        </table>
        <Button variant="ghost" className="mx-auto mt-3 mb-[300px]">
            <CgMoreAlt className="w-[16px] h-[16px]" />
            <span>more</span>
        </Button>
    </ScrollArea>
  )
}

export default SidebarContent
