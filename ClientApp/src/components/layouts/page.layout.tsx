import { cn } from "@/lib/utils/cn.util";
import { ThemeButton } from "@/features/theme/components/theme-button";

export const PageLayout = ({
    children,
    className,
    title,
    description,
}: {
    children: React.ReactNode;
    className?: string;
    title?: string;
    description?: string;
}) => {
    return (
        <>
            <title>{title}</title>
            <meta name="description" content={description} />
            <div className="flex h-screen">
                <main
                    className={cn(
                        "flex flex-col items-center justify-center w-full p-6",
                        className
                    )}
                >
                    {children}
                    <ThemeButton />
                </main>
            </div>
        </>
    );
};
